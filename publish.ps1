msbuild /t:clean /p:configuration=Release
msbuild /p:configuration=Release
if (-not (test-path .\builds))
{
    mkdir builds
}
mono "/Applications/Visual Studio.app/Contents/Resources/lib/monodevelop/bin/vstool.exe" setup pack Ultramarine.UserSecrets/bin/Release/net472/Ultramarine.UserSecrets.dll -d:./builds