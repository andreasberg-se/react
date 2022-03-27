import './App.css';
import React from "react";
import menuClick, { MENU_OPTIONS } from "./components/Menu";

function App() {
  return (
    <div className="App">
      <h1>People</h1>

      <button className="menu" onClick={ e => menuClick(MENU_OPTIONS.TABLE_VIEW) }>Table View</button>
      <button className="menu" onClick={ e => menuClick(MENU_OPTIONS.LIST_VIEW) }>List View</button>
      <button className="menu" onClick={ e => menuClick(MENU_OPTIONS.ADD_PERSON) }>Add person</button>

      <hr />

      <div id="View">[ React App ]</div>

      <hr />

      <div>&copy; 2022, Andreas Berg.</div>

    </div>
  );
}

export default App;

