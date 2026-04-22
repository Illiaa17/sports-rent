

Plan: Full Requirements Implementation (pracktika.txt) 

 Context

 Audit against pracktika.txt. App runs and basic CRUD works. Significant mandatory gaps remain: validation, return flow, fine system,
 search/filter/sort/pagination, user management, role enforcement, ResourceDictionary, localization, image support, and equipment quantity
 tracking.

 ---
 MANDATORY — all 14 gaps

 1. ResourceDictionary (App.axaml)

 Move hardcoded colors into Application.Resources as named SolidColorBrush:
 PrimaryBg=#0F172A, SurfaceBg=#1E2937, Accent=#22C55E, Danger=#EF4444, Muted=#94A3B8, Warning=#EAB308, Info=#3B82F6
 Apply via {StaticResource} in all views.

 2. LocalizationService — complete all keys

 File: Services/LocalizationService.cs
 Add ~50 missing keys covering every label, watermark, button, tab, header, error message in all views (uk + en). Add Loc property to
 BaseViewModel that fires OnPropertyChanged for all loc keys when LanguageChanged fires → XAML binds {Binding Loc[KeyName]}.

 3. Validation

 Files: ViewModels/CustomerViewModel.cs, ViewModels/RentalViewModel.cs
 - Customer phone: 10 digits (^\d{10}$)
 - Customer passport: 2 letters + 6 digits (^[A-ZА-Я]{2}\d{6}$)
 - Rental: DueDate > RentalDate
 - Equipment availability: equipment.Quantity >= requested quantity
 - Show ValidationError string in UI, block save on failure

 4. Customer Search fix

 File: ViewModels/CustomerViewModel.cs
 Implement OnSearchTextChanged — filter by FullName or Phone (same pattern as EquipmentViewModel).

 5. Equipment quantity tracking

 File: ViewModels/RentalViewModel.cs
 - On SaveRental: foreach item → load equipment, decrement Quantity, save equipment.json
 - On Return: restore Quantity

 6. Return flow

 File: ViewModels/RentalViewModel.cs, Views/RentalView.axaml
 - Add ReturnRentalCommand(Rental) — sets ReturnDate=DateTime.Now, Status="Returned", restores quantity, calculates fine if overdue
 - Add Return button column in active rentals DataGrid

 7. Fine system

 Files: Models/AppSettings.cs (new), Services/SettingsService.cs (new), ViewModels/SettingsViewModel.cs, Views/SettingsView.axaml
 - AppSettings has FinePerDay (decimal), Language (string)
 - SettingsService loads/saves Data/settings.json
 - Fine = (ReturnDate - DueDate).Days * FinePerDay added to TotalAmount
 - Fine input in SettingsView

 8. Overdue auto-detection (optional C, already approved)

 In RentalViewModel.LoadData(): for each Active rental where DueDate < DateTime.Now → set Status = "Overdue" and save.

 9. Active rentals filter fix

 File: Views/RentalView.axaml
 First tab DataGrid binds to ActiveRentals (filtered ObservableCollection), not raw Rentals.
 Add status filter ComboBox (All / Active / Returned / Overdue) above list.

 10. Search + Sort + Pagination

 Files: ViewModels/RentalViewModel.cs, ViewModels/EquipmentViewModel.cs
 - Search rentals by customer name or rental ID
 - Sort: enable CanUserSortColumns="True" on all DataGrids
 - Pagination: PageSize (20/50/100) + page nav buttons + PagedItems computed property in both VMs

 11. User Management (Admin-only)

 New: ViewModels/UsersViewModel.cs, Views/UsersView.axaml, Views/UsersView.axaml.cs
 - CRUD: create (BCrypt hash), read, update role/fullName, delete
 - MainViewModel.IsAdmin bound to Users nav button visibility

 12. Role-based visibility

 Files: ViewModels/MainViewModel.cs, Views/MainView.axaml
 - IsAdmin property from AuthService.Instance
 - Users sidebar button: IsVisible="{Binding IsAdmin}"

 13. Image support

 Files: ViewModels/EquipmentViewModel.cs, Views/EquipmentView.axaml
 - Create images/equipment/ on startup
 - "Browse" button → IStorageProvider.OpenFilePickerAsync → copy to images dir, set ImagePath
 - Display <Image> thumbnail in equipment form

 14. Sample seed data

 File: Services/JsonDataService.cs
 Seed 5 equipment items + 3 customers on first run so the app is demonstrable immediately.

 ---
 OPTIONAL — all 5 approved

 ┌─────┬──────────────────────────────────────────────────────────────┐
 │  #  │                           Feature                            │
 ├─────┼──────────────────────────────────────────────────────────────┤
 │ A   │ Confirmation dialogs before every delete                     │
 ├─────┼──────────────────────────────────────────────────────────────┤
 │ B   │ Export to CSV (equipment list + rental history)              │
 ├─────┼──────────────────────────────────────────────────────────────┤
 │ C   │ Automatic overdue detection (covered in #8 above)            │
 ├─────┼──────────────────────────────────────────────────────────────┤
 │ D   │ Today's revenue card on Dashboard                            │
 ├─────┼──────────────────────────────────────────────────────────────┤
 │ E   │ Settings persistence (language + fine rate survive restarts) │
 └─────┴──────────────────────────────────────────────────────────────┘