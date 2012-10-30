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
Call-Agent 'taobao-wf-dev01' 'apploader' ''
#Call-Agent 'taobao-wf-dev01' 'apploader' 'list'
#Call-Agent 'taobao-wf-dev01' 'apploader' 'refresh'
#Call-Agent 'taobao-wf-dev01' 'apploader' 'scan'
#Call-Agent 'taobao-wf-dev01' 'apploader' 'reload dir'