import { useState } from "react";
import TransactionForm from "./components/TransactionForm";
import TransactionHistory from "./components/TransactionHistory";

const App = () => {
  const [userId, setUserId] = useState("");

  return (
    <div>
      <TransactionForm onTransactionSuccess={(id) => setUserId(id)} />
      {userId && <TransactionHistory userId={userId} />}
    </div>
  );
};

export default App;
