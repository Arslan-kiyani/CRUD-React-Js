import React, { useState } from "react";
import CustomTableContainer from "./Components/CustomTableContainer";
import UpsertForm from "./Components/UpsertForm";
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function App() {
  const [isFormVisible, setIsFormVisible] = useState(false);

  const handleAddUserClick = () => {
    setIsFormVisible(true);
  };

  const handleShowGridClick = () => {
    setIsFormVisible(false);
  };

  return (
    <>
    <ToastContainer/>
    <div className="App">
      <div className="d-flex gap-5 mt-2 justify-content-center "style={{ backgroundColor: "#f0f0f0", padding: "2rem"}}>
        <button
          type="button"
          className="btn btn-primary btn-lg mr-1"
          onClick={handleAddUserClick}
        >
          Add User
        </button>
        <button
          type="button"
          className="btn btn-primary btn-lg mr-1"
          onClick={handleShowGridClick}
        >
          Show Grid
        </button>
      </div>
      {isFormVisible ? <UpsertForm /> : <CustomTableContainer />}
    </div>
    </>
  );
}

export default App;
