Set-Location .\Server.Host
dotnet.exe restore
dotnet.exe build -c Release -o ../build
Set-Location ..

