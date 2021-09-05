param  ([double]$ver = 1.3,
        [string]$project = ".\ProjectPangaea.csproj",
        [string]$configuration = "Release" )

$msbuild = 'C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild.exe'
$a = "-p:Configuration=$configuration;RWV=$ver"
& $msbuild $project $a
