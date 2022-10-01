
using AdressBook.Services;

IMenuService menu = new MenuService($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\contacts.json");
// En instans av MenuService skapas och filsökvägen skickas in för att skickas vidare till FileService.
do
{ // Sedan körs MainMenu i en loop.
    menu.MainMenu();
}
while (true);