# פרויקט בנקאות

## סקירה כללית

פרויקט זה כולל **שרת Web API ב-.NET 8** ואת צד הלקוח **ב-React** לניהול פעולות בנקאיות, כולל הפקדות ומשיכות. השרת מתקשר עם ספק בנקאות חיצוני כדי לאמת ולבצע את הפעולות.

## דרישות מוקדמות

לפני הפעלת הפרויקט, ודאו שהתקנתם את הרכיבים הבאים על המחשב שלכם:

- **.NET 8 SDK** 
- **SQL Server (Express או Standard)**
- **Node.js (לצורך התקנת ה-Client)** →
- **Git** (אופציונלי, לניהול גרסאות) →

## מבנה הפרויקט

```
BankingProject/
  ├── BankingApplication/       # השרת הראשי ב-.NET
  ├── ExternalBankingAPI/       # ספק בנקאות חיצוני מדומה
  ├── BankingApp/                   # צד הלקוח ב-React
```

## 🔹 הגדרת ה-Backend (שרת ב-.NET 8)

שני השרתים `` ו-`` דורשים .NET 8 ו-SQL Server.

### 1️⃣ הגדרת מחרוזת החיבור (Connection String)

יש לוודא שה-`Connection String` בקובץ `appsettings.json` מתאים ל-SQL Server שלכם.

דוגמה למחרוזת חיבור (`appsettings.json`):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=BankingDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

אם אתם משתמשים ב-LocalDB, עדכנו ל:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=BankingDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 2️⃣ יצירת מסד הנתונים והחלת המיגרציות

יש להריץ את הפקודה הבאה על מנת ליצור את מסד הנתונים:

```sh
cd BankingApplication
 dotnet ef database update
```

### 3️⃣ הפעלת השרתים

הכינסו לתיקייות של השרתים ותפעילו דרך ה sln
```sh

 dotnet run
```

להפעלת **External API Server**:

```sh
 dotnet run
```

### 4️⃣ בדיקת תקינות ה-APIs

ה-APIs אמורים להיות זמינים בכתובות:

- `https://localhost:44392/swagger` (**שרת הבנקאות הראשי**)
- `https://localhost:44393/swagger` (**ספק בנקאות חיצוני**)

## 🔹 הגדרת צד הלקוח (React Client)

### 1️⃣ התקנת התלויות

```sh
cd BankingApp
npm install
```

### 2️⃣ הפעלת ה-Frontend

```sh
npm run dev
```

### 3️⃣ גישה למערכת

לאחר שה-React Client יפעל, ניתן לגשת אליו בכתובת:

- 'http://localhost:5173/'


