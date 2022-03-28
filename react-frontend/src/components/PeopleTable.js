import React from "react";
import PeopleTableHeader from "./PeopleTableHeader";
import PeopleTableBody from "./PeopleTableBody";

const PeopleTable = (props) => {
  return (
    <table>
      <PeopleTableHeader />
      <PeopleTableBody sortMethod={props.sortMethod} />
    </table>
  );
};

export default PeopleTable;

