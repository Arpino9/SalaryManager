// 基本
global using System;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.ComponentModel;
global using System.Drawing.Imaging;
global using System.Drawing.Text;
global using System.Globalization;
global using System.Linq;
global using System.IO;
global using System.Reactive.Linq;
global using System.Text;
global using System.Text.RegularExpressions;
global using System.Threading.Tasks;
global using System.Windows;
global using System.Windows.Controls;
global using System.Windows.Data;
global using System.Windows.Forms;
global using System.Windows.Media;
global using System.Windows.Input;

// Domain層
global using SalaryManager.Domain;
global using SalaryManager.Domain.Entities;
global using SalaryManager.Domain.Exceptions;
global using SalaryManager.Domain.Modules.Logics;
global using SalaryManager.Domain.Modules.Helpers;
global using SalaryManager.Domain.Repositories;
global using SalaryManager.Domain.StaticValues;
global using SalaryManager.Domain.ValueObjects;

// Infrastructure層
global using SalaryManager.Infrastructure.Google_Calendar;
global using SalaryManager.Infrastructure.Excel;
global using SalaryManager.Infrastructure.JSON;
global using SalaryManager.Infrastructure.Interface;
global using SalaryManager.Infrastructure.PDF;
global using SalaryManager.Infrastructure.SQLite;
global using SalaryManager.Infrastructure.SpreadSheet;
global using SalaryManager.Infrastructure.XML;

// WPF層
global using SalaryManager.WPF.Interface;
global using SalaryManager.WPF.Models;
global using SalaryManager.WPF.ViewModels;
global using SalaryManager.WPF.Window;

// API
global using Reactive.Bindings;