# **ACID in SQL Server**

## **Atomicity**

* **Definition**: A transaction must be **all or nothing**.
  If one part fails, the entire transaction is rolled back.
* **SQL Server**:

  * Guarantees atomicity through **explicit transactions** (`BEGIN TRAN`, `COMMIT`, `ROLLBACK`).
* **NoSQL**:

  * Some databases provide limited atomicity (e.g., document/collection-level in MongoDB).
  * Unlike SQL, they typically cannot guarantee atomicity across multiple collections/tables.

---

## **Consistency**

* **Definition**: After a transaction, the database must remain in a **valid state**, following integrity rules (constraints, data types, foreign keys, etc.).
* **In SQL Server**: Constraints, triggers, relationships, default values, and business rules enforce consistency.

---

## **Isolation**

* **Definition**: Defines how and when changes made by a transaction are visible to other transactions.
* **Isolation levels in SQL Server**:

  1. **Read Uncommitted**

     * Allows **dirty reads** (reading uncommitted data from another transaction).
     * No read locks.

  2. **Read Committed** *(default in SQL Server)*

     * Prevents dirty reads.
     * Uses row locks (or row versioning if `READ_COMMITTED_SNAPSHOT` is enabled).

  3. **Repeatable Read**

     * Prevents dirty reads and **non-repeatable reads**.
     * Holds locks on read rows until the end of the transaction.

  4. **Serializable**

     * The most restrictive level.
     * Prevents dirty reads, non-repeatable reads, and **phantom reads**.
     * May use range locks to block inserts/updates that would create new rows in the scanned range.

  5. **Snapshot Isolation** *(optional, must be enabled)*

     * Uses a **version store** in `tempdb`.
     * Avoids blocking reads.
     * Ensures a consistent view of data as of the transaction start time.

---

## **Durability**

* **Definition**: Once a transaction is committed, the data **must persist**, even in the event of a failure.
* **How SQL Server ensures durability**:

  * **Transaction Log (WAL - Write Ahead Log)**: writes are logged before being applied to the database.
  * On failure, SQL Server recovers the consistent state from the logs.

---

# **Database Anomalies**

* **Dirty Read**: Reading uncommitted data.
* **Non-Repeatable Read**: The same query returns different results because another transaction modified the data.
* **Phantom Read**: A repeated query sees new rows inserted by another transaction.

---
