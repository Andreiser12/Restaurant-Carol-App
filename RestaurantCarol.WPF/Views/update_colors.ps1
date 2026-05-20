$files = Get-ChildItem -Path "c:\Users\andre\Desktop\Anul 2\MAP\Restaurant-Carol-App\RestaurantCarol.WPF\Views" -Filter *.xaml -Recurse

foreach ($file in $files) {
    # Specify UTF8 encoding explicitly
    $content = Get-Content $file.FullName -Encoding UTF8
    $modified = $false
    
    if ($content -match 'Background="White"') {
        $content = $content -replace 'Background="White"', 'Background="#373334"'
        $modified = $true
    }
    
    if ($content -match 'Foreground="White"') {
        $content = $content -replace 'Foreground="White"', 'Foreground="#D2AF6D"'
        $modified = $true
    }

    if ($content -match 'Background="#F5F5F5"') {
        $content = $content -replace 'Background="#F5F5F5"', 'Background="#373334"'
        $modified = $true
    }

    if ($content -match 'Background="#F8F8F8"') {
        $content = $content -replace 'Background="#F8F8F8"', 'Background="#373334"'
        $modified = $true
    }
    
    if ($content -match 'Background="#FDF1DC"') {
        $content = $content -replace 'Background="#FDF1DC"', 'Background="#373334"'
        $modified = $true
    }
    
    if ($content -match 'Background="#F4B860"') {
        $content = $content -replace 'Background="#F4B860"', 'Background="#D2AF6D"'
        $modified = $true
    }
    
    if ($content -match 'Background="Black"') {
        $content = $content -replace 'Background="Black"', 'Background="#373334"'
        $modified = $true
    }

    if ($content -match 'Foreground="Black"') {
        $content = $content -replace 'Foreground="Black"', 'Foreground="#D2AF6D"'
        $modified = $true
    }

    if ($modified) {
        # Specify UTF8 encoding explicitly to avoid corruption of ←, ș, ț, etc.
        Set-Content -Path $file.FullName -Value $content -Encoding UTF8
        Write-Host "Updated $($file.Name)"
    }
}
