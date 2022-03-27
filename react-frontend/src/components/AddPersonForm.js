import React, { Component } from 'react';
import ReactDOM from "react-dom";
import AddPerson from "./AddPerson";

class AddPersonForm extends Component {
  constructor(props) {
    super(props);
    this.state = {
      FirstName: '',
      LastName: '',
      CityId: 1,
      cities: [],
      error: null,
      isLoaded: false
    };
  }

  componentDidMount() {
    fetch("/api/react/cities")
    .then(response => response.json())
    .then(
      (result) => {
        this.setState({
          isLoaded: true,
          cities: result
        });
      },
      (error) => {
        this.setState({
          isLoaded: true,
          error
        })
      },
    )
  }

  changeInput = (event) => {
    this.setState({[event.target.name]: event.target.value});
  };

  submitForm = (event) => {
    event.preventDefault();
    ReactDOM.render(<AddPerson data={this.state} />, document.getElementById('View'));
  };

  render() {
    const {error, isLoaded, cities} = this.state;
    if (error) {
      return <div>Loading error!</div>
    } else if (!isLoaded) {
      return <div>Loading ...</div>
    } else {
      const getCities = cities.map((row, index) => {
        return (
          <option key={index} value={row.CityId}>{row.Name}</option>
        );
      });

      return (
        <form onSubmit={this.submitForm} method="post">
          <label>First Name *</label>
          <input type="text" value={this.state.value} name="FirstName" onChange={this.changeInput} required />
          <label>Last Name *</label>
          <input type="text" value={this.state.value} name="LastName" onChange={this.changeInput} required />
          <label>City *</label>
          <select value={this.state.value} name="CityId" onChange={this.changeInput}>{getCities}</select>
          <button type="submit">Add Person</button>
        </form> 
      );
    }
  }
}

export default AddPersonForm;

