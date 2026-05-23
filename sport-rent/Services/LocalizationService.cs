using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace sport_rent.Services;

public class LocalizationService : INotifyPropertyChanged
{
    public static LocalizationService Instance { get; } = new();
    public event Action? LanguageChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    private string _lang = "uk";
    public string CurrentLang
    {
        get => _lang;
        set
        {
            if (_lang == value) return;
            _lang = value;
            LanguageChanged?.Invoke();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }
    }

    private readonly Dictionary<string, Dictionary<string, string>> _dict = new()
    {
        ["uk"] = new()
        {
            // Auth
            {"Login", "Вхід"}, {"LoginWatermark", "Логін"}, {"PasswordWatermark", "Пароль"},
            {"LoginBtn", "Увійти"}, {"AppTitle", "Прокат Спорт"},
            {"AppSubtitle", "Автоматизація пункту прокату"},
            {"FillAllFields", "Заповніть всі поля"}, {"InvalidCredentials", "Невірні облікові дані"},
            // Navigation
            {"Dashboard", "Головна"}, {"Equipment", "Спорядження"}, {"Customers", "Клієнти"},
            {"Rentals", "Оренди"}, {"Settings", "Налаштування"}, {"Logout", "Вийти"}, {"Users", "Користувачі"},
            // Dashboard
            {"DashboardTitle", "Головна панель"}, {"TotalEquipmentLabel", "Усього спорядження"},
            {"ActiveRentalsLabel", "Активних оренд"}, {"TotalCustomersLabel", "Клієнтів"},
            {"TodayRevenueLabel", "Дохід сьогодні (грн)"},
            // Equipment
            {"EquipmentTitle", "Спорядження"}, {"SearchEquipmentWatermark", "Пошук по назві або категорії"},
            {"AddEquipmentBtn", "Додати спорядження"}, {"SaveBtn", "Зберегти"}, {"DeleteBtn", "Видалити"},
            {"BrowseBtn", "Огляд..."}, {"ExportCsvBtn", "Експорт CSV"},
            {"NameWatermark", "Назва"}, {"CategoryWatermark", "Категорія"},
            {"DailyRateWatermark", "Ціна/день"}, {"DepositWatermark", "Застава"},
            {"QuantityWatermark", "Кількість"}, {"ConditionWatermark", "Стан"},
            {"ImageSection", "Зображення"}, {"PageSizeLabel", "На сторінці:"},
            {"PrevPage", "◀"}, {"NextPage", "▶"},
            {"AddEquipmentTitle", "Нове спорядження"},
            {"EditEquipmentTitle", "Редагування спорядження #{0}"},
            // Customers
            {"CustomersTitle", "Клієнти"}, {"SearchCustomerWatermark", "Пошук клієнта"},
            {"AddCustomerBtn", "Додати клієнта"}, {"FullNameWatermark", "ПІБ"},
            {"PhoneWatermark", "Телефон (10 цифр)"}, {"PassportWatermark", "Паспорт (АА123456)"},
            {"AddCustomerTitle", "Новий клієнт"},
            {"EditCustomerTitle", "Редагування клієнта #{0}"},
            // Rentals
            {"RentalsTitle", "Оренди / Повернення"}, {"AllRentalsTab", "Всі оренди"},
            {"NewRentalTab", "Нова оренда"}, {"CustomerLabel", "Клієнт"},
            {"RentalDateLabel", "Дата оренди"}, {"DueDateLabel", "Термін повернення"},
            {"AddItemSection", "Додати позицію"}, {"QuantityLabel", "Кількість"},
            {"DaysLabel", "Днів"}, {"AddItemBtn", "Додати"}, {"SaveRentalBtn", "Зберегти оренду"},
            {"ReturnBtn", "Повернути"}, {"StatusFilterLabel", "Статус:"}, {"AllStatuses", "Всі"},
            {"SearchRentalWatermark", "Пошук за клієнтом або ID"},
            {"DepositLabel", "Застава:"}, {"ExportRentalsCsvBtn", "Експорт CSV"},
            {"NewRentalTitle", "Нова оренда"},
            {"RentalItemsHeader", "Позиції оренди"},
            // Settings
            {"SettingsTitle", "Налаштування"}, {"LanguageSection", "Мова інтерфейсу"},
            {"ThemeSection", "Тема"}, {"ToggleThemeBtn", "Перемкнути тему"},
            {"FineSection", "Штрафи"}, {"FinePerDayLabel", "Штраф за день прострочення (грн):"},
            {"DamageFinePercentLabel", "Штраф за пошкодження (% від застави):"},
            {"ThemeDark", "Темна"}, {"ThemeLight", "Світла"}, {"CurrentThemeLabel", "Поточна:"},
            // Users
            {"UsersTitle", "Користувачі"}, {"RoleLabel", "Роль"}, {"NewPasswordWatermark", "Новий пароль"},
            {"LoginFieldWatermark", "Логін"}, {"AddUserTitle", "Новий користувач"},
            {"EditUserTitle", "Редагування користувача #{0}"},
            // Validation
            {"InvalidPhone", "Невірний формат телефону (10 цифр)"},
            {"InvalidPassport", "Невірний формат паспорту (2 літери + 6 цифр)"},
            {"InvalidDateRange", "Дата повернення має бути після дати оренди"},
            {"InsufficientQty", "Недостатня кількість спорядження на складі"},
            {"SelectCustomer", "Оберіть клієнта"}, {"SelectEquipment", "Оберіть спорядження"},
            {"AddItemFirst", "Додайте хоча б одну позицію"},
            {"LoginRequired", "Логін обов'язковий"},
            {"PasswordRequired", "Пароль обов'язковий для нового користувача"},
            // Dialogs
            {"ConfirmDelete", "Підтвердити видалення"}, {"ConfirmDeleteMsg", "Видалити цей запис?"},
            {"ReturnConfirmTitle", "Підтвердити повернення"}, {"ReturnConfirmMsg", "Оформити повернення цього обладнання?"},
            {"ReturnDialogTitle", "Повернення обладнання"},
            {"ReturnDialogSubtitle", "Оренда #{0} — {1}"},
            {"ReturnDatesSection", "Дати"},
            {"ReturnDueDateLabel", "Термін повернення:"},
            {"ReturnDateLabel", "Дата фактичного повернення:"},
            {"ReturnCheckCondition", "Стан спорядження при поверненні:"},
            {"ReturnItemLine", "{0} ×{1} — стан: {2}"},
            {"ReturnDamagedMark", "Пошкоджено"},
            {"ReturnOverdueInfo", "Прострочення: {0} дн. × {1} грн = {2} грн"},
            {"ReturnOnTime", "Повернення в термін."},
            {"ReturnApplyOverdueFine", "Нарахувати штраф за прострочення ({0} грн)"},
            {"ReturnDamageInfo", "Штраф за пошкодження: {0} грн"},
            {"ReturnApplyDamageFine", "Нарахувати штраф за пошкодження ({0} грн)"},
            {"ReturnNoDamageFine", "Пошкоджень не відмічено."},
            {"ReturnFinePreview", "Штрафи: прострочення {0} + пошкодження {1} = {2} грн\nДо сплати разом: {3} грн"},
            {"ConfirmReturnBtn", "Оформити повернення"},
            {"ConfirmLogoutTitle", "Вийти з системи"}, {"ConfirmLogoutMsg", "Ви впевнені, що хочете вийти?"},
            {"ConfirmClearTitle", "Очистити форму"}, {"ConfirmClearMsg", "Скинути поточні дані нової оренди?"},
            {"ConfirmSaveTitle", "Підтвердити збереження"}, {"ConfirmSaveMsg", "Зберегти ці зміни?"},
            {"ConfirmBtn", "Підтвердити"}, {"CancelBtn", "Скасувати"},
            // Status
            {"StatusActive", "Активна"}, {"StatusReturned", "Повернена"}, {"StatusOverdue", "Прострочена"},
            // Misc
            {"PageLabel", "Стор."}, {"OfLabel", "з"},
            {"NewBtn", "Нове"}, {"NoData", "Немає даних"}, {"NoSelection", "Нічого не обрано"},
            {"ClearBtn", "Очистити"},
            // DataGrid headers
            {"ColId", "ID"}, {"ColName", "Назва"}, {"ColCategory", "Категорія"},
            {"ColDailyRate", "Ціна/день"}, {"ColDeposit", "Застава"}, {"ColQty", "К-сть"},
            {"ColCondition", "Стан"}, {"ColCustomer", "Клієнт"}, {"ColRentalDate", "Оренда"},
            {"ColDueDate", "Термін"}, {"ColReturnDate", "Повернення"}, {"ColStatus", "Статус"},
            {"ColTotal", "Сума (грн)"}, {"ColFines", "Штрафи"}, {"ColAction", "Дія"},
            {"ColPhone", "Телефон"}, {"ColPassport", "Паспорт"}, {"ColFullName", "ПІБ"},
            {"ColLogin", "Логін"}, {"ColRole", "Роль"}, {"ColEquipment", "Спорядження"},
            {"ColDays", "Днів"}, {"ColRate", "Ставка"}, {"ColSubtotal", "Підсумок"},
        },
        ["en"] = new()
        {
            // Auth
            {"Login", "Login"}, {"LoginWatermark", "Login"}, {"PasswordWatermark", "Password"},
            {"LoginBtn", "Sign In"}, {"AppTitle", "Sport Rent"},
            {"AppSubtitle", "Rental shop automation"},
            {"FillAllFields", "Fill all fields"}, {"InvalidCredentials", "Invalid credentials"},
            // Navigation
            {"Dashboard", "Dashboard"}, {"Equipment", "Equipment"}, {"Customers", "Customers"},
            {"Rentals", "Rentals"}, {"Settings", "Settings"}, {"Logout", "Logout"}, {"Users", "Users"},
            // Dashboard
            {"DashboardTitle", "Dashboard"}, {"TotalEquipmentLabel", "Total Equipment"},
            {"ActiveRentalsLabel", "Active Rentals"}, {"TotalCustomersLabel", "Customers"},
            {"TodayRevenueLabel", "Today Revenue (UAH)"},
            // Equipment
            {"EquipmentTitle", "Equipment"}, {"SearchEquipmentWatermark", "Search by name or category"},
            {"AddEquipmentBtn", "Add Equipment"}, {"SaveBtn", "Save"}, {"DeleteBtn", "Delete"},
            {"BrowseBtn", "Browse..."}, {"ExportCsvBtn", "Export CSV"},
            {"NameWatermark", "Name"}, {"CategoryWatermark", "Category"},
            {"DailyRateWatermark", "Daily Rate"}, {"DepositWatermark", "Deposit"},
            {"QuantityWatermark", "Quantity"}, {"ConditionWatermark", "Condition"},
            {"ImageSection", "Image"}, {"PageSizeLabel", "Per page:"},
            {"PrevPage", "◀"}, {"NextPage", "▶"},
            {"AddEquipmentTitle", "New Equipment"},
            {"EditEquipmentTitle", "Edit equipment #{0}"},
            // Customers
            {"CustomersTitle", "Customers"}, {"SearchCustomerWatermark", "Search customer"},
            {"AddCustomerBtn", "Add Customer"}, {"FullNameWatermark", "Full Name"},
            {"PhoneWatermark", "Phone (10 digits)"}, {"PassportWatermark", "Passport (AA123456)"},
            {"AddCustomerTitle", "New Customer"},
            {"EditCustomerTitle", "Edit customer #{0}"},
            // Rentals
            {"RentalsTitle", "Rentals / Returns"}, {"AllRentalsTab", "All Rentals"},
            {"NewRentalTab", "New Rental"}, {"CustomerLabel", "Customer"},
            {"RentalDateLabel", "Rental Date"}, {"DueDateLabel", "Due Date"},
            {"AddItemSection", "Add Item"}, {"QuantityLabel", "Quantity"},
            {"DaysLabel", "Days"}, {"AddItemBtn", "Add"}, {"SaveRentalBtn", "Save Rental"},
            {"ReturnBtn", "Return"}, {"StatusFilterLabel", "Status:"}, {"AllStatuses", "All"},
            {"SearchRentalWatermark", "Search by customer or ID"},
            {"DepositLabel", "Deposit:"}, {"ExportRentalsCsvBtn", "Export CSV"},
            {"NewRentalTitle", "New Rental"},
            {"RentalItemsHeader", "Rental Items"},
            // Settings
            {"SettingsTitle", "Settings"}, {"LanguageSection", "Interface Language"},
            {"ThemeSection", "Theme"}, {"ToggleThemeBtn", "Toggle Theme"},
            {"FineSection", "Fines"}, {"FinePerDayLabel", "Fine per overdue day (UAH):"},
            {"DamageFinePercentLabel", "Damage fine (% of deposit):"},
            {"ThemeDark", "Dark"}, {"ThemeLight", "Light"}, {"CurrentThemeLabel", "Current:"},
            // Users
            {"UsersTitle", "Users"}, {"RoleLabel", "Role"}, {"NewPasswordWatermark", "New password"},
            {"LoginFieldWatermark", "Login"}, {"AddUserTitle", "New User"},
            {"EditUserTitle", "Edit user #{0}"},
            // Validation
            {"InvalidPhone", "Invalid phone format (10 digits)"},
            {"InvalidPassport", "Invalid passport format (2 letters + 6 digits)"},
            {"InvalidDateRange", "Due date must be after rental date"},
            {"InsufficientQty", "Insufficient equipment quantity in stock"},
            {"SelectCustomer", "Select a customer"}, {"SelectEquipment", "Select equipment"},
            {"AddItemFirst", "Add at least one item"},
            {"LoginRequired", "Login is required"},
            {"PasswordRequired", "Password is required for a new user"},
            // Dialogs
            {"ConfirmDelete", "Confirm Delete"}, {"ConfirmDeleteMsg", "Delete this record?"},
            {"ReturnConfirmTitle", "Confirm Return"}, {"ReturnConfirmMsg", "Process return for this rental?"},
            {"ReturnDialogTitle", "Equipment Return"},
            {"ReturnDialogSubtitle", "Rental #{0} — {1}"},
            {"ReturnDatesSection", "Dates"},
            {"ReturnDueDateLabel", "Due date:"},
            {"ReturnDateLabel", "Actual return date:"},
            {"ReturnCheckCondition", "Equipment condition on return:"},
            {"ReturnItemLine", "{0} ×{1} — condition: {2}"},
            {"ReturnDamagedMark", "Damaged"},
            {"ReturnOverdueInfo", "Overdue: {0} day(s) × {1} UAH = {2} UAH"},
            {"ReturnOnTime", "Returned on time."},
            {"ReturnApplyOverdueFine", "Charge overdue fine ({0} UAH)"},
            {"ReturnDamageInfo", "Damage fine: {0} UAH"},
            {"ReturnApplyDamageFine", "Charge damage fine ({0} UAH)"},
            {"ReturnNoDamageFine", "No damage marked."},
            {"ReturnFinePreview", "Fines: overdue {0} + damage {1} = {2} UAH\nTotal due: {3} UAH"},
            {"ConfirmReturnBtn", "Process Return"},
            {"ConfirmLogoutTitle", "Sign out"}, {"ConfirmLogoutMsg", "Are you sure you want to sign out?"},
            {"ConfirmClearTitle", "Clear form"}, {"ConfirmClearMsg", "Discard current new-rental data?"},
            {"ConfirmSaveTitle", "Confirm Save"}, {"ConfirmSaveMsg", "Save these changes?"},
            {"ConfirmBtn", "Confirm"}, {"CancelBtn", "Cancel"},
            // Status
            {"StatusActive", "Active"}, {"StatusReturned", "Returned"}, {"StatusOverdue", "Overdue"},
            // Misc
            {"PageLabel", "Page"}, {"OfLabel", "of"},
            {"NewBtn", "New"}, {"NoData", "No data"}, {"NoSelection", "Nothing selected"},
            {"ClearBtn", "Clear"},
            // DataGrid headers
            {"ColId", "ID"}, {"ColName", "Name"}, {"ColCategory", "Category"},
            {"ColDailyRate", "Daily Rate"}, {"ColDeposit", "Deposit"}, {"ColQty", "Qty"},
            {"ColCondition", "Condition"}, {"ColCustomer", "Customer"}, {"ColRentalDate", "Rental"},
            {"ColDueDate", "Due"}, {"ColReturnDate", "Returned"}, {"ColStatus", "Status"},
            {"ColTotal", "Total (UAH)"}, {"ColFines", "Fines"}, {"ColAction", "Action"},
            {"ColPhone", "Phone"}, {"ColPassport", "Passport"}, {"ColFullName", "Full Name"},
            {"ColLogin", "Login"}, {"ColRole", "Role"}, {"ColEquipment", "Equipment"},
            {"ColDays", "Days"}, {"ColRate", "Rate"}, {"ColSubtotal", "Subtotal"},
        }
    };

    public string this[string key] => _dict[_lang].GetValueOrDefault(key, key);
}
