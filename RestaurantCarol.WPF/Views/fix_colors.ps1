$files = Get-ChildItem -Path "c:\Users\andre\Desktop\Anul 2\MAP\Restaurant-Carol-App\RestaurantCarol.WPF\Views" -Filter *.xaml -Recurse

foreach ($file in $files) {
    # Specify UTF8 encoding explicitly
    $content = Get-Content $file.FullName -Raw -Encoding UTF8
    
    # Replace Background="#D2AF6D" Foreground="#D2AF6D" with Foreground="#373334" if they are on the same line or next line
    $newContent = [System.Text.RegularExpressions.Regex]::Replace($content, 'Background="#D2AF6D"(.*?)Foreground="#D2AF6D"', 'Background="#D2AF6D"$1Foreground="#373334"', [System.Text.RegularExpressions.RegexOptions]::Singleline)
    
    $newContent = [System.Text.RegularExpressions.Regex]::Replace($newContent, 'Foreground="#D2AF6D"(.*?)Background="#D2AF6D"', 'Foreground="#373334"$1Background="#D2AF6D"', [System.Text.RegularExpressions.RegexOptions]::Singleline)
    
    if ($newContent -ne $content) {
        Set-Content -Path $file.FullName -Value $newContent -Encoding UTF8
        Write-Host "Fixed $($file.Name)"
    }
}
