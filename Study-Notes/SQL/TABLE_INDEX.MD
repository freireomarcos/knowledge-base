# 📚 Indexes in SQL Server

## 🔹 What are indexes?

An **index** is a data structure used by SQL Server to locate rows more efficiently, instead of scanning the entire table.
Internally, indexes are stored as a **B-Tree (Balanced Tree)**, which allows quick navigation from the root node down to the leaf nodes.

📖 Analogy: Think of a book index → instead of reading every page, you go directly to the right chapter.

---

## 🔹 Clustered Index

* Defines the **physical order** of the table rows.
* A table can only have **one clustered index**.
* The **leaf level of the clustered index = the actual data rows**.

### Example

```sql
-- Table with a clustered index on Id (PK creates clustered by default)
CREATE TABLE Person (
    Id INT PRIMARY KEY,
    Name NVARCHAR(100),
    Age INT,
    CreatedAt DATETIME,
    Email NVARCHAR(200)
);

-- Create clustered index manually on another column
CREATE CLUSTERED INDEX IX_Person_CreatedAt
ON Person (CreatedAt);
```

### How it works

* `SELECT * FROM Person WHERE Id = 10`
  → SQL Server navigates through the clustered index tree, finds the row directly, returns all columns.

---

## 🔹 Non-Clustered Index

* A **separate structure** from the table.
* Stores:

  * The **indexed key(s)**.
  * A **pointer to the clustered index row** (or heap if no clustered index exists).
* A table can have **multiple non-clustered indexes**.

### Example

```sql
-- Non-clustered index on Name
CREATE NONCLUSTERED INDEX IX_Person_Name
ON Person (Name);
```

### How it works

* `SELECT Name FROM Person WHERE Name = 'Alice'`
  → Index lookup is efficient.
* `SELECT Name, Email FROM Person WHERE Name = 'Alice'`
  → If `Email` is not in the index, SQL Server performs a **Key Lookup** into the clustered index to fetch it.

---

## 🔹 Covering Index (INCLUDE)

You can **include extra columns** in a non-clustered index so queries don’t need lookups.

### Example

```sql
CREATE NONCLUSTERED INDEX IX_Person_CreatedAt
ON Person (CreatedAt)
INCLUDE (Email);
```

* `SELECT CreatedAt, Email FROM Person WHERE CreatedAt = '2025-09-23'`
  → Fully resolved by the index, no lookup needed.
* `SELECT CreatedAt, Name FROM Person WHERE CreatedAt = '2025-09-23'`
  → Lookup required to fetch `Name`.

---

## 🔹 Heap (no clustered index)

If a table has **no clustered index**, data rows are stored unordered as a **heap**.
Heaps are generally slower for lookups because SQL Server may need to scan the entire table.

---

## 🔹 Practical Examples

### 1. Clustered Index Seek

```sql
SELECT * 
FROM Person 
WHERE Id = 1;
```

✅ Efficient lookup, directly uses clustered index.

---

### 2. Non-Clustered Seek + Key Lookup

```sql
SELECT Name, Age 
FROM Person 
WHERE Name = 'Alice';
```

* `IX_Person_Name` finds candidates.
* SQL Server performs a **Key Lookup** to fetch `Age`.

---

### 3. Covering Index

```sql
SELECT CreatedAt, Email
FROM Person
WHERE CreatedAt = '2025-09-23';
```

* Fully covered by `IX_Person_CreatedAt INCLUDE (Email)`.
* ✅ No lookup required.

---

### 4. Table Scan

```sql
SELECT Name
FROM Person
WHERE Name LIKE '%Test%';
```

* Because of the leading `%`, the index cannot be used effectively.
* SQL Server performs a full table scan.

---

## 🔹 Best Practices

✅ Index columns frequently used in `WHERE`, `JOIN`, and `ORDER BY`.
✅ Use `INCLUDE` to avoid Key Lookups in read-heavy queries.
⚠️ Avoid too many indexes → `INSERT/UPDATE/DELETE` operations get more expensive.
⚠️ Favor indexes on **selective columns** (ones that filter down many rows).

---

# 🚀 Summary

* **Clustered Index** = the table itself, physically ordered.
* **Non-Clustered Index** = separate structure with pointers back to clustered.
* **INCLUDE** = adds extra columns to avoid lookups.
* **Heap** = unordered table without clustered index.