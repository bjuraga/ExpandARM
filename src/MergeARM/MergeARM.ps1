Param (
	[string]
	$fileToExpand
)

function Main(){
	# load file to expand
	$fileContent = LoadARM -file $fileToExpand

	# expand file:// links
	$expandedFile = ExpandFile $fileContent
	# store file to originalName.expanded.json
}

function LoadARM($file){
	return ( "{" + (Get-Content $file) + "}") | ConvertFrom-Json
}

function ExpandFile($fileContent){
	return $fileContent
}
