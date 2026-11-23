param (
    [string]$Path = "."
)

Write-Host "Scanning C# files for interval candidate patterns in: $Path"

$patterns = @(
    '(DateTimeOffset|DateTime|DateOnly|TimeOnly|int|double)\s+\w*(Start|From|Begin)\w*;',
    '(DateTimeOffset|DateTime|DateOnly|TimeOnly|int|double)\s+\w*(End|To)\w*;',
    '>=\s*\w*(Start|From|Begin)\w*\s*&&\s*<=\s*\w*(End|To)\w*',
    '<=\s*\w*(End|To)\w*\s*&&\s*>=\s*\w*(Start|From|Begin)\w*'
)

Get-ChildItem -Path $Path -Filter *.cs -Recurse | ForEach-Object {
    $file = $_.FullName
    $lines = Get-Content $file
    for ($i = 0; $i -lt $lines.Count; $i++) {
        foreach ($pattern in $patterns) {
            if ($lines[$i] -match $pattern) {
                Write-Output "$file`:Line $($i + 1): $($lines[$i].Trim())"
            }
        }
    }
}
