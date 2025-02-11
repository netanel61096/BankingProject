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
  try {
    const response = await axios.put(`${API_URL}/update/${transactionId}`, { newAmount, newBankAccount });
    return response.data;
  } catch (error) {
    throw error.response ? error.response.data : { message: "עדכון נכשל" };
  }
};

export const deleteTransaction = async (transactionId) => {
  try {
    const response = await axios.delete(`${API_URL}/delete/${transactionId}`);
    return response.data;
  } catch (error) {
    throw error.response ? error.response.data : { message: "מחיקה נכשלה" };
  }
};

