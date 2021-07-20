using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Assimp;
using Microsoft.Win32;

namespace NetEngine.Toolkit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _selectedFile;
        private string SelectedFile
        {
            get => _selectedFile;
            set
            {
                SelectionTextBox.Text = _selectedFile = value;
            }
        }

        private string _outputFile;
        public string OutputFile
        {
            get => _outputFile;
            set
            {
                OutputTextBox.Text = _outputFile = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() ?? false)
            {
                SelectedFile = dialog.FileName;
            }
        }

        private void SelectOutputButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() ?? false)
            {
                OutputFile = dialog.FileName;
            }
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedFile) || string.IsNullOrEmpty(OutputFile))
            {
                MessageBox.Show(
                    "Must select both an input and output file.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            using var importer = new AssimpContext();
            var scene = importer.ImportFile(SelectedFile);

            // For now, a very naive approach: just grab the first mesh from the Assimp scene
            var mesh = scene?.Meshes?.FirstOrDefault();
            if (mesh == null
                || !mesh.HasVertices
                || !mesh.HasFaces
                || !mesh.HasNormals)
            {
                return;
            }

            using var writer = new BinaryWriter(File.OpenWrite(OutputFile));
            writer.Write((byte)1); // Version byte
            writer.Write(mesh.VertexCount); // Vertex count
            for (var i = 0; i < mesh.VertexCount; ++i)
            {
                var pos = mesh.Vertices[i];
                var norm = mesh.Normals[i];
                var bitangent = mesh.HasTangentBasis ? mesh.BiTangents[i] : new Vector3D(0.0F);
                // TODO: using normal, bitangent, and tangent to generate a tangent-space quaternion
                var tangentSpace = new Quaternion();
                var texCoord = mesh.HasTextureCoords(0)
                    ? mesh.TextureCoordinateChannels[0][i]
                    : new Vector3D(0.0F);
                // Position
                writer.Write(pos.X);
                writer.Write(pos.Y);
                writer.Write(pos.Z);
                // Tangent space
                writer.Write(tangentSpace.X);
                writer.Write(tangentSpace.Y);
                writer.Write(tangentSpace.Z);
                writer.Write(tangentSpace.W);
                // Texture coords
                writer.Write(texCoord.X);
                writer.Write(texCoord.Y);
            }
            writer.Write(mesh.FaceCount * 3); // Index count
            foreach (var face in mesh.Faces)
            {
                foreach (var index in face.Indices)
                {
                    writer.Write(index);
                }
            }
        }
    }
}
