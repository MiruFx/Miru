
set artifacts_dir=%cd%\scripts\MiruNewTest

pushd %TEMP%

rmdir /S /Q Mong

cmd /c mirucli new Mong

pushd Mong

dotnet build

cmd /c miru assets:setup
cmd /c miru db:create 
cmd /c miru db:create -e test
cmd /c miru make:feature TopupNew Topups New
cmd /c miru make:entity Topup

echo f | xcopy %artifacts_dir%\201911071845_CreateTopup.cs src\Mong\Database\Migrations\201911071845_CreateTopup.cs
echo y | xcopy %artifacts_dir%\MongDbContext.cs src\Mong\Database\MongDbContext.cs
echo y | xcopy %artifacts_dir%\New.cshtml src\Mong\Features\Topups\New.cshtml
echo y | xcopy %artifacts_dir%\Topup.cs src\Mong\Domain\Topup.cs
echo y | xcopy %artifacts_dir%\TopupNew.cs src\Mong\Features\Topups\TopupNew.cs
echo y | xcopy %artifacts_dir%\TopupNewTest.cs tests\Mong.Tests\Features\Topups\TopupNewTest.cs
echo y | xcopy %artifacts_dir%\TopupNewPageTest.cs tests\Mong.PageTests\Pages\Topups\TopupNewPageTest.cs

dotnet build

cmd /c miru test
cmd /c miru pagetest

echo YESSSSS!

popd
popd