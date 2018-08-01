if (Test-Path('./build')) {
    Set-Location ./build 
    Remove-Item -Recurse -Force ./*
    Set-Location ..
}
else {
    mkdir build
}

Set-Location ./server
dotnet.exe restore
dotnet.exe publish -o ../../build/server
Set-Location ../client
npm run build
Set-Location ..
Copy-Item ./client/build -Recurse ./build/client


