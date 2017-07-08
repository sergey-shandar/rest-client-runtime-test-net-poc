$Dir = get-childitem "Microsoft.Rest.ClientRuntime.Test\bin\Debug\"
$List = $Dir | where {$_.extension -eq ".nupkg"}
$File = $List | where {-not $_.Name.EndsWith(".symbols.nupkg")}
Remove-Item $File.FullName
$Symbols = $List | where {$_.Name.EndsWith(".symbols.nupkg")}
Rename-Item $Symbols.FullName $File.FullName