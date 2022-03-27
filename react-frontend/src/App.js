import './App.css';
import React from "react";
import PeopleDisplay from "./components/PeopleDisplay";
import PeopleTableHeader from "./components/PeopleTableHeader";
import PeopleTableBody from "./components/PeopleTableBody";
import AddPersonForm from "./components/AddPersonForm";

function App() {
  return (
    <div className="App">
      <h1>People</h1>

      <div id="Details"></div>

      <hr />
      <p>[Styled Component]</p>
      <div id="List">
      <PeopleDisplay></PeopleDisplay>
      </div>

      <hr />
      <p>[Table Component]</p>
      <div id="Table">
        <table>    
          <PeopleTableHeader />
          <PeopleTableBody />  
        </table>
      </div>

      <hr />
      <p>[Add Person]</p>
      <div id="Form">
      <AddPersonForm />
      </div>
    </div>
  );
}

export default App;

