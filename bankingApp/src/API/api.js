import axios from "axios";

const API_URL = "https://localhost:44392/api/transactions";

export const executeTransaction = async (transactionData) => {
  try {
    const response = await axios.post(`${API_URL}/execute`, transactionData);
    return response.data;
  } catch (error) {
    throw error.response ? error.response.data : { message: "Network error" };
  }
};

export const getTransactionHistory = async (userId) => {
  try {
    const response = await axios.get(`${API_URL}/history/${userId}`);
    return response.data;
  } catch (error) {
    throw error.response ? error.response.data : { message: "Network error" };
  }
};

export const updateTransaction = async (transactionId, newAmount, newBankAccount) => {
    const response = await fetch(`${API_URL}/update/${transactionId}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ newAmount, newBankAccount }),
    });
  
    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || "עדכון נכשל");
    }
    return response.json();
  };
  
  // שליחת בקשת מחיקה
  export const deleteTransaction = async (transactionId) => {
    const response = await fetch(`${API_URL}/delete/${transactionId}`, {
      method: "DELETE",
    });
  
    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || "מחיקה נכשלה");
    }
    return response.json();
  };
