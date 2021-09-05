param  ([double]$ver = 1.1,
        [double]$latest = 1.0,
        [double]$bottom = 1.0,
        [string]$project = ".\ProjectPangaea.csproj",
        [string]$configuration = "Release" )

if ($ver -eq $latest){
    $msbuild = 'C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild.exe'
    $dif = 0.1
    for ($i = $ver - $dif; $i -gt $bottom; $i -= $dif){
        $a = "-p:Configuration=$configuration;RWV=$i"
        & $msbuild $project $a
    }
}
