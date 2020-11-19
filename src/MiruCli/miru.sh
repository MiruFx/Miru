#!/bin/bash
# ~/.dotnet/tools/miru

runApp()
{
    project_dir=$(MiruCli $project)
    dotnet run -p $project_dir miru $*
}

runTest()
{
    project_dir=$(MiruCli $project)
    dotnet run -p $project_dir miru $*
}

runCli()
{
    MiruCli $*
}

runVersion()
{
    MiruCli --version
}

runMiru()
{
    project_dir=$(MiruCli $project) && dotnet run -p $project_dir --no-build miru $*
}

runAt()
{
    project_dir=$(MiruCli $project)
    (cd $project_dir && $2 $3 $4 $5 $6 $7 $8 $9)
}

case $1 in  
    "run-app") project="app"; runApp $*;;
    "@app") project="app"; runAt $*;;
    "run-test") project="tests"; runTest $*;;
    "@test") project="tests"; runAt $*;;
    "run-pagetest") project="pagetests"; runTest $*;;
    "@pagetest") project="pagetests"; runAt $*;;
    "new") runCli $*;;
    "--version") runVersion $*;;
    "-v") runVersion $*;;
    *) project="app"; runMiru $*;;
esac