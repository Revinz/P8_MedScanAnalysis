using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityVolumeRendering;

public class DICOMLoader
{
    public VolumeDataset LoadFolder(string folderPath) {
        bool recursive = true;

        // Read all files
        IEnumerable<string> fileCandidates = Directory.EnumerateFiles(folderPath, "*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
            .Where(p => p.EndsWith(".dcm", StringComparison.InvariantCultureIgnoreCase) || p.EndsWith(".dicom", StringComparison.InvariantCultureIgnoreCase) || p.EndsWith(".dicm", StringComparison.InvariantCultureIgnoreCase));

        // Import the dataset
        DICOMImporter importer = new DICOMImporter(fileCandidates, Path.GetFileName(folderPath));
        VolumeDataset dataset = importer.Import();

        return dataset;
    }
}
