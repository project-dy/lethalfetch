# powershell
# 임시디렉터리 생성

#rmdir .\temp -Recurse -Force 2>null
mkdir -p .\temp

# 파일 복사
copy .\bin\Debug\net46\LethalFetch.dll .\temp\LethalFetch.dll

copy manifest.json .\temp
copy CHANGELOG.md .\temp
copy README.md .\temp
copy icon.png .\temp

zip.exe -r .\LethalFetch.zip .\temp
# rmdir .\temp -Recurse -Force

# 복사
#copy .\temp %appdata%\r2modmanPlus-local\LethalCompany\profiles\Default\BepInEx\plugins\pdy-LethalFetch\temp -Recurse -Force
