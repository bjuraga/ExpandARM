New-Fixture deploy Clean

$here = (Join-Path -Path (Split-Path (Split-Path (Split-Path -Parent $MyInvocation.MyCommand.Path))) -ChildPath '\src\MergeARM')
$sut = (Split-Path -Leaf $MyInvocation.MyCommand.Path) -replace '\.Tests\.', '.'
$script = "$here\$sut"
. "$script"

Describe "LoadFile" {
	$fileContent = LoadARM -file '.\tests\Merge.ARM.Tests\sampleTemplate\Template.json'

    It "has a content" {
		$fileContent | Should -Not -Be $null
    }
}
