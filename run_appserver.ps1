.\build.bat appserver
start build\appserver\AppServer.Console.exe
Start-Sleep -s 2

echo '\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\'
echo 'call appserver...'

#appagent client func
function Call-Agent($s,$n,$msg){
    $c=New-Object System.IO.Pipes.NamedPipeClientStream($s,$n,3)
    $c.Connect(100)

    $w=New-Object System.IO.StreamWriter($c)
    $r=New-Object System.IO.StreamReader($c)

    $w.WriteLine($msg)
    $w.Flush()

    do{
        $i=$r.ReadLine();
        Write-Host $i
    }While(![System.String]::IsNullOrEmpty($i))
}

#sample
Call-Agent 'localhost' 'appserver' ''
Call-Agent 'localhost' 'appserver' 'list'