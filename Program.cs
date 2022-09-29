
using AdressBook.Services;

IMenuService menu = new MenuService($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\contacts.json");

do
{
    menu.MainMenu();
}
while (true);