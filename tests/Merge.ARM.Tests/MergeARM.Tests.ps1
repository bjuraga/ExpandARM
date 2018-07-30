New-Fixture deploy Clean

$here = (Join-Path -Path (Split-Path (Split-Path (Split-Path -Parent $MyInvocation.MyCommand.Path))) -ChildPath '\src\MergeARM')
$sut = (Split-Path -Leaf $MyInvocation.MyCommand.Path) -replace '\.Tests\.', '.'
$script = "$here\$sut"
. "$script"

Describe "LoadFile" {
    It "loads the file" {
		$fileContent = LoadARM -file '.\tests\Merge.ARM.Tests\sampleTemplate\Template.json'

		$fileContent.resources[0].apiVersion | Should BeExactly '2017-05-10'
    }
}
