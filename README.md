# Introduction
Tool that enables you to use local file system template links

# Currently supported:
- Use fully qualified file://C:/template.json links in the templateLink url
- Supports forward slash directory separator
- Currently the expanded ARM template is stored with adding '.expanded' between the file name and extension.
	Example: C:\template.expanded.json
	If the file already exists, it will be owerriden. 

# Planned future features (roadmap):
- Expand nested templates
- Option to save nested templates to filesystem also
- Use relative paths in the file:// links
- Support for parameterLink expansion
- Detect endless loops
- Provide an option to not owerride a file on save, but generate a new one?