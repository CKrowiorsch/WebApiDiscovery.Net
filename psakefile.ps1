$erroractionpreference = "Stop"
properties {
    $location = (get-location);
    $outdir = (join-path $location "Build");
    $bindir = (join-path $outdir "Bin");
}

task default -depends Help
task deply -depends Help


task Help {
    write-host "To Create Nuget-Packages Locally: psake.cmd 'Create:Nuget'" -ForegroundColor Magenta
}


task Clean {
	rmdir -force -recurse $outdir -ea SilentlyContinue
}

task Rebuild -depends Clean {
    dotnet build -c Release -r win-x86 -clp:nosummary -v:m -o "$bindir\WebApiDiscovery.Net\" "Source/WebApiDiscovery.Net/WebApiDiscovery.Net.csproj" -nologo
}
task Create:Nuget -depends Clean {
    dotnet build -c Release -r win-x86 -clp:nosummary -v:m -o "$bindir\WebApiDiscovery.Net\" "Source/WebApiDiscovery.Net/WebApiDiscovery.Net.csproj" -nologo
}

