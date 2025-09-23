# **ACID no SQL Server**

## **Atomicidade**

* **Definição**: A transação deve ser **tudo ou nada**.
  Se uma parte falhar, toda a transação é desfeita (rollback).
* **SQL Server**:

  * Garante atomicidade via **transações explícitas** (`BEGIN TRAN`, `COMMIT`, `ROLLBACK`).
* **NoSQL**:

  * Alguns bancos oferecem atomicidade limitada (ex.: nível de documento/coleção no MongoDB).
  * Diferente do SQL, não garante atomicidade em múltiplas coleções/tabelas.

---

## **Consistência**

* **Definição**: Após uma transação, o banco deve permanecer em um **estado válido**, obedecendo regras de integridade (constraints, tipos de dados, chaves estrangeiras, etc).
* **No SQL Server**: Constraints, triggers, relacionamentos, default values e regras de negócios ajudam a garantir consistência.

---

## **Isolamento**

* **Definição**: Define como e quando as alterações feitas por uma transação podem ser visíveis para outras.
* **Níveis no SQL Server**:

  1. **Read Uncommitted**

     * Permite **dirty reads** (lê dados não confirmados de outra transação).
     * Sem locks de leitura.
  2. **Read Committed (padrão do SQL Server)**

     * Evita dirty reads.
     * Usa locks de linha (ou row versioning com `READ_COMMITTED_SNAPSHOT`).
  3. **Repeatable Read**

     * Evita dirty reads e **non-repeatable reads**.
     * Mantém locks nas linhas lidas até o final da transação.
  4. **Serializable**

     * O mais restritivo.
     * Evita dirty reads, non-repeatable reads e **phantom reads**.
     * Pode usar locks de intervalo, evitando inserções/alterações que gerem novos registros no range lido.
  5. **Snapshot Isolation** *(opcional, precisa ser habilitado)*

     * Usa **version store** no tempdb.
     * Evita bloqueios de leitura.
     * Garante leitura consistente no momento em que a transação começou.

---

## **Durabilidade**

* **Definição**: Uma vez confirmado (`COMMIT`), o dado **deve persistir**, mesmo em caso de falha.
* **Como o SQL Server garante**:

  * **Transaction Log (WAL - Write Ahead Log)**: grava antes de aplicar no banco.
  * Em falhas, o SQL Server recupera o estado consistente a partir dos logs.

---

# **Database Anomalies**

* **Dirty Read**: Lê dados não confirmados.
* **Non-Repeatable Read**: Mesma consulta retorna resultados diferentes, pois outra transação alterou os dados.
* **Phantom Read**: Novas linhas aparecem em uma consulta repetida, porque outra transação inseriu dados durante a execução.

---

# **Plano de Execução no SQL Server**

* Mostra **como o otimizador de consultas executa um SQL**.
* Ferramenta: `SET SHOWPLAN_ALL ON`, `SET STATISTICS IO ON` ou plano gráfico no SSMS.

## **Índices**

1. **Clustered Index**

   * Ordena fisicamente os dados na tabela.
   * Só pode haver **1 por tabela**.
   * Exemplo: `PRIMARY KEY` geralmente cria um clustered.
   * Estrutura: **B-Tree** (Balanced Tree).

2. **Non-Clustered Index**

   * Estrutura separada da tabela.
   * Contém chave(s) de índice + ponteiro para localizar o dado real.
   * Pode haver vários por tabela.
   * **Key Lookup**: Quando um índice não contém todas as colunas necessárias → o otimizador precisa buscar no clustered index.

     * Mitigação: **Índices cobertos (covering index)** com `INCLUDE`.
