// ***********************************************************************
// Copyright (c) 2015-2018 Charlie Poole, Rob Prouse
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System.IO;
using NUnit.Common;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnitLite;

namespace Miru.Testing
{
    /// <remarks>
    /// dotnet run 
    /// </remarks>
    public class MiruTestRunner : ITestListener
    {
        public const int OK = 0;
        public const int INVALID_ARG = -1;
        public const int FILE_NOT_FOUND = -2;
        public const int INVALID_TEST_FIXTURE = -4;
        public const int UNEXPECTED_ERROR = -100;

        private Assembly _testAssembly;
        private ITestAssemblyRunner _runner;

        private NUnitLiteOptions _options;
        private ITestListener _teamCity = null;

        private TextUI _textUI;

        public MiruTestRunner(Assembly testAssembly)
        {
            _testAssembly = testAssembly;
        }

        public ResultSummary Summary { get; private set; }

        public int Execute(string[] args)
        {
            TestMiruHost.UsingTestRunner = true;
            
            _options = new NUnitLiteOptions(_testAssembly == null, args);

            ExtendedTextWriter outWriter = null;
            if (_options.OutFile != null)
            {
                var outFile = Path.Combine(_options.WorkDirectory, _options.OutFile);
                var textWriter = TextWriter.Synchronized(new StreamWriter(outFile));
                outWriter = new ExtendedTextWrapper(textWriter);
                Console.SetOut(outWriter);
            }
            else
            {
                outWriter = new ColorConsoleWriter(!_options.NoColor);
            }

            using (outWriter)
            {
                TextWriter errWriter = null;
                if (_options.ErrFile != null)
                {
                    var errFile = Path.Combine(_options.WorkDirectory, _options.ErrFile);
                    errWriter = TextWriter.Synchronized(new StreamWriter(errFile));
                    Console.SetError(errWriter);
                }

                using (errWriter)
                {
                    _textUI = new TextUI(outWriter, Console.In, _options);
                    return Execute();
                }
            }
        }

        // // Entry point called by AutoRun and by the .NET Standard nunitlite.runner
        // public int Execute(ExtendedTextWriter writer, TextReader reader, string[] args)
        // {
        //     _options = new NUnitLiteOptions(_testAssembly == null, args);
        //
        //     _textUI = new TextUI(writer, reader, _options);
        //
        //     return Execute();
        // }

        // Internal Execute depends on _textUI and _options having been set already.
        private int Execute()
        {
            _runner = new NUnitTestAssemblyRunner(new DefaultTestAssemblyBuilder());

            try
            {
                if (_options.ErrorMessages.Count > 0)
                {
                    _textUI.DisplayErrors(_options.ErrorMessages);
                    _textUI.Writer.WriteLine();
                    _textUI.DisplayHelp();

                    return TextRunner.INVALID_ARG;
                }

                if (_testAssembly == null && _options.InputFile == null)
                {
                    _textUI.DisplayError("No test assembly was specified.");
                    _textUI.Writer.WriteLine();
                    _textUI.DisplayHelp();

                    return TextRunner.OK;
                }

                var testFile = _testAssembly != null
                    ? AssemblyHelper.GetAssemblyPath(_testAssembly)
                    : _options.InputFile;

                if (_testAssembly == null)
                    _testAssembly = AssemblyHelper.Load(testFile);

                if (_options.WaitBeforeExit && _options.OutFile != null)
                    _textUI.DisplayWarning("Ignoring /wait option - only valid for Console");

                var runSettings = NUnitLite.TextRunner.MakeRunSettings(_options);
                LoadTests(runSettings);

                // We display the filters at this point so that any exception message
                // thrown by CreateTestFilter will be understandable.
                _textUI.DisplayTestFilters();

                TestFilter filter = TextRunner.CreateTestFilter(_options);

                return RunTests(filter, runSettings);
            }
            catch (FileNotFoundException ex)
            {
                _textUI.DisplayError(ex.Message);
                return FILE_NOT_FOUND;
            }
            catch (Exception ex)
            {
                _textUI.DisplayError(ex.ToString());
                return UNEXPECTED_ERROR;
            }
            finally
            {
                if (_options.WaitBeforeExit)
                    _textUI.WaitForUser("Press Enter key to continue . . .");
            }
        }

        private void LoadTests(IDictionary<string, object> runSettings)
        {
            _runner.Load(_testAssembly, runSettings);
        }

        public int RunTests(TestFilter filter, IDictionary<string, object> runSettings)
        {
            var startTime = DateTime.UtcNow;

            ITestResult result = _runner.Run(this, filter);

            ReportResults(result);

            if (_options.ResultOutputSpecifications.Count > 0)
            {
                var outputManager = new OutputManager(_options.WorkDirectory);

                foreach (var spec in _options.ResultOutputSpecifications)
                    outputManager.WriteResultFile(result, spec, runSettings, filter);
            }
            if (Summary.InvalidTestFixtures > 0)
                return INVALID_TEST_FIXTURE;

            return Summary.FailureCount + Summary.ErrorCount + Summary.InvalidCount;
        }

        public void ReportResults(ITestResult result)
        {
            Summary = new ResultSummary(result);

            if (Summary.ExplicitCount + Summary.SkipCount + Summary.IgnoreCount > 0)
                _textUI.DisplayNotRunReport(result);

            if (result.ResultState.Status == TestStatus.Failed || result.ResultState.Status == TestStatus.Warning)
                _textUI.DisplayErrorsFailuresAndWarningsReport(result);

            // _textUI.DisplayRunSettings();

            // _textUI.DisplaySummaryReport(Summary);
            
            // _textUI.Writer.WriteLine("Summary:");
            // _textUI.Writer.WriteLine();

            if (Summary.FailedCount > 0)
            {
                _textUI.Writer.WriteLine(ColorStyle.Error, "\tFAILED");
                _textUI.Writer.WriteLine();
                _textUI.Writer.WriteLine($"\t {Summary.FailedCount} tests failed");
                _textUI.Writer.WriteLine($"\t {Summary.PassCount} tests passed");
            }
            else
            {
                _textUI.Writer.WriteLine(ColorStyle.Pass, "\tPASSED");
                _textUI.Writer.WriteLine();
            }
            
            _textUI.Writer.WriteLine($"\tTotal of {Summary.TestCount} tests run in {Summary.Duration:F0}s");
            _textUI.Writer.WriteLine();
        }

        /// <summary>
        /// Called when a test or suite has just started
        /// </summary>
        /// <param name="test">The test that is starting</param>
        public void TestStarted(ITest test)
        {
            if (_teamCity != null)
                _teamCity.TestStarted(test);

            _textUI.TestStarted(test);
        }

        /// <summary>
        /// Called when a test has finished
        /// </summary>
        /// <param name="result">The result of the test</param>
        public void TestFinished(ITestResult result)
        {
            if (_teamCity != null)
                _teamCity.TestFinished(result);

            _textUI.TestFinished(result);
        }

        /// <summary>
        /// Called when a test produces output for immediate display
        /// </summary>
        /// <param name="output">A TestOutput object containing the text to display</param>
        public void TestOutput(TestOutput output)
        {
            _textUI.TestOutput(output);
        }

        /// <summary>
        /// Called when a test produces a message to be sent to listeners
        /// </summary>
        /// <param name="message">A TestMessage object containing the text to send</param>
        public void SendMessage(TestMessage message)
        {
            
        }
    }
}