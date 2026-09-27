// System
global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Configuration;
global using System.Data;
global using System.Data.SqlClient;
global using System.Drawing;
global using System.Drawing.Imaging;
global using System.Globalization;
global using System.IO;
global using System.Linq;
global using System.Reactive.Linq;
global using System.Reflection;
global using System.Text.RegularExpressions;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Windows;
global using System.Windows.Controls;
global using System.Windows.Forms;

// Domain層
global using SalaryManager.Domain.Entities;
global using SalaryManager.Domain.Exceptions;
global using SalaryManager.Domain.Modules.Helpers;
global using SalaryManager.Domain.Modules.Logics;
global using SalaryManager.Domain.Repositories;
global using SalaryManager.Domain.ValueObjects;

// Third Party
global using Microsoft.Data.Sqlite;
global using Reactive.Bindings;
global using SixLabors.Fonts;
global using SixLabors.ImageSharp;
global using SixLabors.ImageSharp.Formats;
global using SixLabors.ImageSharp.Formats.Bmp;
global using SixLabors.ImageSharp.PixelFormats;
global using SixLabors.ImageSharp.Formats.Gif;
global using SixLabors.ImageSharp.Formats.Jpeg;
global using SixLabors.ImageSharp.Formats.Png;
global using SixLabors.ImageSharp.Formats.Tiff;
