import { useState } from "react";
import { useForm } from "react-hook-form";
import { executeTransaction } from "../API/api";
import styles from "./TransactionForm.module.css";

const TransactionForm = ({ onTransactionSuccess }) => {
  const { register, handleSubmit, formState: { errors } } = useForm();
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");

  const onSubmit = async (data) => {
    setLoading(true);
    setMessage("");
    try {
      const response = await executeTransaction(data);
      setMessage(response.message);
      onTransactionSuccess(data.userId);
    } catch (error) {
      setMessage(error.message || "Transaction failed");
    }
    setLoading(false);
  };

  return (
    <div className={styles.container}>
      <h2>בצע פעולה</h2>
      {message && <p className={styles.message}>{message}</p>}
      <form onSubmit={handleSubmit(onSubmit)}>

        <input 
          {...register("fullNameHebrew", {
            required: "שם בעברית הוא שדה חובה",
            pattern: {
              value: /^[א-ת\s'\-]{1,20}$/,
              message: "שם בעברית חייב להכיל רק אותיות בעברית, עד 20 תווים"
            }
          })}
          placeholder="שם בעברית"
        />
        {errors.fullNameHebrew && <span>{errors.fullNameHebrew.message}</span>}

        <input 
          {...register("fullNameEnglish", {
            required: "שם באנגלית הוא שדה חובה",
            pattern: {
              value: /^[A-Za-z\s'\-]{1,20}$/,
              message: "שם באנגלית חייב להכיל רק אותיות באנגלית, עד 20 תווים"
            }
          })}
          placeholder="שם באנגלית"
        />
        {errors.fullNameEnglish && <span>{errors.fullNameEnglish.message}</span>}

        <input 
          type="date" 
          {...register("dateOfBirth", { required: "תאריך לידה הוא שדה חובה" })} 
        />
        {errors.dateOfBirth && <span>{errors.dateOfBirth.message}</span>}

        <input {...register("userId", { 
          required: "תעודת זהות היא חובה",
          pattern: { value: /^\d{9}$/, message: "תעודת זהות חייבת להכיל 9 ספרות בלבד" }
        })} 
          placeholder="ת.ז (9 ספרות)" 
        />
        {errors.userId && <span>{errors.userId.message}</span>}

        <select {...register("actionType", { required: "יש לבחור פעולה" })}>
          <option value="">בחר פעולה</option>
          <option value="deposit">הפקדה</option>
          <option value="withdrawal">משיכה</option>
        </select>
        {errors.actionType && <span>{errors.actionType.message}</span>}

        <input {...register("amount", { 
          required: "יש להזין סכום", 
          pattern: { value: /^\d{1,10}$/, message: "סכום חייב להיות מספר חוקי, עד 10 ספרות וללא תווים מיוחדים" }
        })} 
          placeholder="סכום" type="number" 
        />
        {errors.amount && <span>{errors.amount.message}</span>}

        <input {...register("bankAccount", { 
          required: "יש להזין מספר חשבון", 
          pattern: { value: /^\d{1,10}$/, message: "מספר חשבון חייב להכיל עד 10 ספרות וללא תווים מיוחדים" }
        })} 
          placeholder="מספר חשבון (עד 10 ספרות)" 
        />
        {errors.bankAccount && <span>{errors.bankAccount.message}</span>}

        <button type="submit" disabled={loading}>
          {loading ? "מעבד..." : "בצע פעולה"}
        </button>
      </form>
    </div>
  );
};

export default TransactionForm;
