# NikonImporter

A very basic script that imports the selected shots from my Nikon camera to my computer.

## Usage

```
    NikonImporter.exe list.txt source_path dest_path
    - list.txt : Text file containing each file number to be copied,
                 one per line. Eg: 123 for DSC_0123.JPG
                 Add * after number to also inclure RAW image.
                 Eg: 123* for DSC_0123.JPG and DSC_0123.NEF
    - source_path : Folder containing originals
    - dest_path   : Destination folder

## Compatibility

Windows with .NET 4.5