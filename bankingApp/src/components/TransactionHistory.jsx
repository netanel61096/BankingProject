import { useEffect, useState } from "react";
import { getTransactionHistory, updateTransaction, deleteTransaction } from "../API/api";
import styles from "./TransactionHistory.module.css";

const TransactionHistory = ({ userId }) => {
  const [transactions, setTransactions] = useState([]);
  const [editingTransaction, setEditingTransaction] = useState(null);
  const [newAmount, setNewAmount] = useState("");
  const [newBankAccount, setNewBankAccount] = useState("");
  const [message, setMessage] = useState("");

  useEffect(() => {
    if (userId) {
      getTransactionHistory(userId)
        .then(setTransactions)
        .catch(console.error);
    }
  }, [userId]);

  const startEditing = (transaction) => {
    setEditingTransaction(transaction.id);
    setNewAmount(transaction.amount);
    setNewBankAccount(transaction.bankAccount);
  };

  const cancelEditing = () => {
    setEditingTransaction(null);
    setNewAmount("");
    setNewBankAccount("");
  };

  const saveEdit = async (transactionId) => {
    try {
      const response = await updateTransaction(transactionId, newAmount, newBankAccount);
      setMessage(response.message);

      setTransactions((prevTransactions) =>
        prevTransactions.map((tx) =>
          tx.id === transactionId ? { ...tx, amount: newAmount, bankAccount: newBankAccount } : tx
        )
      );
      cancelEditing();
    } catch (error) {
      setMessage(error.message);
    }
  };

  const handleDeleteTransaction = async (transactionId) => {
    try {
      await deleteTransaction(transactionId);
      setTransactions((prevTransactions) => prevTransactions.filter((tx) => tx.id !== transactionId));
      setMessage(" הפעולה נמחקה בהצלחה.");
    } catch (error) {
      setMessage(error.message);
    }
  };

  return (
    <div className={styles.container}>
      <h2>היסטוריית פעולות</h2>
      {message && <p className={styles.message}>{message}</p>}
      <ul>
        {transactions.length === 0 ? (
          <p>אין פעולות להצגה</p>
        ) : (
          transactions.map((tx) => (
            <li key={tx.id} className={styles.transactionItem}>
              {editingTransaction === tx.id ? (
                <>
                  <div>
                  <input
                    type="number"
                    value={newAmount}
                    onChange={(e) => setNewAmount(e.target.value)}
                    className={styles.input}
                  />
                  <input
                    type="text"
                    value={newBankAccount}
                    onChange={(e) => setNewBankAccount(e.target.value)}
                    className={styles.input}
                  />
                  </div>
                  <div>
                  <button className={styles.saveButton} onClick={() => saveEdit(tx.id)}>
                     שמור
                  </button>
                  <button className={styles.cancelButton} onClick={cancelEditing}>
                     ביטול
                  </button>
                  </div>
                </>
              ) : (
                <>
                  {tx.actionType === "deposit" ? "הפקדה" : "משיכה"} - {tx.amount}₪ | חשבון: {tx.bankAccount} | מצב: {tx.status}
                 <div className={styles.Buttons}>
                 <button className={styles.editButton} onClick={() => startEditing(tx)}>
                     עריכה
                  </button>
                  <button className={styles.deleteButton} onClick={() => handleDeleteTransaction(tx.id)}>
                     מחיקה
                  </button>
                 </div>
                </>
              )}
            </li>
          ))
        )}
      </ul>
    </div>
  );
};

export default TransactionHistory;
