import React, { Component } from "react";
import ReactDOM from "react-dom";
import PeopleDisplay from "./PeopleDisplay";
import PeopleTableHeader from "./PeopleTableHeader";
import PeopleTableBody from "./PeopleTableBody";
import AddPersonForm from "./AddPersonForm";

class AddPerson extends Component {
  constructor(props) {
    super(props);
    this.state = {
      error: null,
      isAdded: false
    };
  }

  componentDidMount() {
    const data = this.props.data;
    fetch("/api/react", { 
      method: 'POST',
      headers: { 
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        FirstName: data.FirstName,
        LastName: data.LastName,
        CityId: parseInt(data.CityId)
      })
    })
    .then(
      (result) => {
        this.setState({
          isAdded: true
        });
      },
      (error) => {
        this.setState({
          isAdded: true,
          error
        })
      },
    )
  }

  render() {
    const {error, isAdded} = this.state;
    if (error) {
      return <div>Could not add person!</div>
    } else if (!isAdded) {
      return <div>Adding ...</div>
    } else {
      ReactDOM.unmountComponentAtNode(document.getElementById('List'));
      ReactDOM.render(<PeopleDisplay />, document.getElementById('List'));
      ReactDOM.unmountComponentAtNode(document.getElementById('Table'));
      ReactDOM.render(<table><PeopleTableHeader /><PeopleTableBody /></table>,
       document.getElementById('Table'));
      ReactDOM.unmountComponentAtNode(document.getElementById('Form'));
      ReactDOM.render(<AddPersonForm />, document.getElementById('Form'));
      return (
        <article className="add">
          <h2>Person is added!</h2>
        </article>
      )
    }
  }

};

export default AddPerson;

