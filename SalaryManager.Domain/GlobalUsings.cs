// 基本
global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.Data.SqlClient;
global using System.Drawing;
global using System.Drawing.Imaging;
global using System.Globalization;
global using System.IO;
global using System.Linq;
global using System.Reactive.Linq;
global using System.Reflection;
global using System.Text.RegularExpressions;
global using System.Threading.Tasks;
global using System.Windows;
global using System.Windows.Forms;
global using System.Windows.Media;
global using System.Windows.Media.Imaging;

// Domain層
global using SalaryManager.Domain.Entities;
global using SalaryManager.Domain.Exceptions;
global using SalaryManager.Domain.Modules.Helpers;
global using SalaryManager.Domain.Repositories;
global using SalaryManager.Domain.ValueObjects;

// 外部API
global using Microsoft.Data.Sqlite;
global using Reactive.Bindings;