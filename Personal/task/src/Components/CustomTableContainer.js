import React, { useEffect, useState } from "react";
import {
  DelteProfilesById,
  GetAllProfiles,
  GetLanguages,
  GetRaces,
  UpdateProfileById,
} from "../Api";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { MultiSelect } from "react-multi-select-component";
import InputMask from "react-input-mask";
import ConfirmationModal from "./ConfirmationModal"; // Adjust the import path
import { Modal, Button } from "react-bootstrap"; // Import Modal and Button from react-bootstrap


function CustomTableContainer() {
  const [data, setData] = useState([]);
  const [editProfile, setEditProfile] = useState(null);
  const [showEditForm, setShowEditForm] = useState(false);

  const [allLanguages, setAllLanguages] = useState([]);
  const [allRaces, setAllRaces] = useState([]);
  const [selected, setSelected] = useState([]);
  const [isChange, setIsChange] = useState(0);
  const [showModal, setShowModal] = useState(false);
  const [deleteProfileId, setDeleteProfileId] = useState(null);
  

  useEffect(() => {
    GetAllProfiles().then((response) => {
      if (response) {
        console.log("Success Response : ", response);
        setData(response);
      } else {
        console.log("Error: ", response);
      }
    });

    GetLanguages().then((response) => {
      if (response) {
        console.log("Success Response : ", response);
        setAllLanguages(response);
      } else {
        console.log("Error: ", response);
      }
    });

    GetRaces().then((response) => {
      if (response) {
        console.log("Success Response : ", response);
        setAllRaces(response);
      } else {
        console.log("Error: ", response);
      }
    });
  }, [isChange]);

  const options = allLanguages.map((language) => ({
    label: language.name,
    value: language.name,
  }));

  const handleDeleteClick = (id) => {
    setDeleteProfileId(id);
    setShowModal(true);
  };

  const handleConfirmDelete = () => {
    DelteProfilesById(deleteProfileId)
      .then((response) => {
        if (response) {
          setData(data.filter((profile) => profile.id !== deleteProfileId));
          toast.success("Profile deleted successfully");
        } else {
          console.log("Error: ", response);
          toast.error("Failed to delete profile");
        }
      })
      .catch((error) => {
        console.log("Error: ", error);
        toast.error("Failed to delete profile");
      });
    setShowModal(false);
    setDeleteProfileId(null);
  };

  const handleEditClick = (profile) => {
    try {
        setEditProfile(profile);
        
        const selectedLanguages = allLanguages
          .filter((lang) => profile.languages.includes(lang.name))
          .map((lang) => ({ label: lang.name, value: lang.name }));

        setSelected(selectedLanguages);
        setShowEditForm(true);
    } catch (error) {
        console.error("Error handling edit click:", error);
       
    }
};


  const languageNames = selected.map((language) => language.label);
  console.log(languageNames);

  const handleEditSubmit = (event) => {
    event.preventDefault();
    UpdateProfileById(editProfile.id, editProfile, languageNames).then(
      (response) => {
        if (response) {
          setData(
            data.map((profile) =>
              profile.id === editProfile.id ? editProfile : profile
            )
          );
          toast.success("Profile updated successfully");
          setShowEditForm(false);
          setEditProfile(null);
          setIsChange(isChange + 1);
        } else {
          console.log("Error: ", response);
          toast.error("Failed to update profile");
        }
      }
    );
  };

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setEditProfile({ ...editProfile, [name]: value });
  };

  function truncateString(str, maxLength) {
    if (!str) return "";
    if (str.length <= maxLength) return str;
    return str.substring(0, maxLength - 3) + "...";
  }

  return (
    <div>
      
      <div className="container">
        <div className="row">
          <div className="col-12">
          <h1 className="text-left mt-2">Show All Profile</h1>
            <table className="table table-bordered table-striped table-hover mt-4">
              <thead className="thead-dark">
                <tr>
                  <th scope="col">Email</th>
                  <th scope="col">First Name</th>
                  <th scope="col">Last Name</th>
                  <th scope="col">Mobile No</th>
                  <th scope="col">Gender</th>
                  <th scope="col">Marital Status</th>
                  <th scope="col">Address</th>
                  <th scope="col">Zip code</th>
                  <th scope="col">City</th>
                  <th scope="col">State</th>
                  <th scope="col">Race</th>
                  <th scope="col">Languages</th>
                  <th scope="col">Actions</th>
                </tr>
              </thead>
              <tbody>
                {data.map((profile) => (
                  <tr key={profile.id}>
                    <td>{profile.email}</td>
                    <td>{profile.firstName}</td>
                    <td>{profile.lastName}</td>
                    <td>{profile.mobileNo}</td>
                    <td>{profile.gender}</td>
                    <td>{profile.maritalStatus}</td>
                    <td>{truncateString(profile.address, 20)}</td>
                    <td>{profile.zip}</td>
                    <td>{profile.city}</td>
                    <td>{profile.state}</td>
                    <td>{profile.race}</td>
                    <td>{profile.languages}</td>
                    <td>
                      <div
                        className="btn-group"
                        role="group"
                        aria-label="Action buttons"
                      >
                        <button
                          type="button"
                          className="btn btn-success btn-sm mr-1"
                          onClick={() => handleEditClick(profile)}
                        >
                          <i className="fas fa-edit" />
                        </button>
                        <button
                          type="button"
                          className="btn btn-danger btn-sm mr-1"
                          onClick={() => handleDeleteClick(profile.id)}
                        >
                          <i className="far fa-trash-alt" />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>
      
      {/* Edit Profile Modal */}
      <Modal show={showEditForm} onHide={() => setShowEditForm(false)} size="xl">
        <Modal.Header closeButton>
          <Modal.Title>Edit Profile</Modal.Title>
        </Modal.Header>
        <Modal.Body className="bg-light">
          {editProfile && (
            <form onSubmit={handleEditSubmit}>
              <div className="row mt-3">
                <div className="col-6">
                  <label>First Name</label>
                  <input
                    className="form-control"
                    name="firstName"
                    value={editProfile.firstName}
                    onChange={handleInputChange}
                  />
                </div>
                <div className="col-6">
                  <label>Last Name</label>
                  <input
                    className="form-control"
                    name="lastName"
                    value={editProfile.lastName}
                    onChange={handleInputChange}
                  />
                </div>
              </div>
              <div className="row mt-3">
                <div className="col-6">
                  <label>Email</label>
                  <input
                    className="form-control"
                    name="email"
                    value={editProfile.email}
                    onChange={handleInputChange}
                  />
                </div>
                <div className="col-6">
                  <label>Mobile No</label>
                  <InputMask
                    mask="(999) 999-9999"
                    className="form-control"
                    name="mobileNo"
                    value={editProfile.mobileNo}
                    onChange={handleInputChange}
                  >
                    {(inputProps) => <input {...inputProps} type="text" />}
                  </InputMask>
                </div>
              </div>
              <div className="row mt-3">
                <div className="col-6">
                  <label>Address</label>
                  <input
                    className="form-control"
                    name="address"
                    value={editProfile.address}
                    onChange={handleInputChange}
                  />
                </div>
                <div className="col-6">
                  <label>City</label>
                  <input
                    className="form-control"
                    name="city"
                    value={editProfile.city}
                    onChange={handleInputChange}
                  />
                </div>
              </div>
              <div className="row mt-3">
                <div className="col-6">
                  <label>State</label>
                  <input
                    className="form-control"
                    name="state"
                    value={editProfile.state}
                    onChange={handleInputChange}
                  />
                </div>
                <div className="col-6">
                  <label>Zip Code</label>
                  <input
                    type="text"
                    className="form-control"
                    name="zip"
                    value={editProfile.zip}
                    onChange={handleInputChange}
                  />
                </div>
              </div>
              <div className="row mt-3">
                <div className="col-6">
                  <label>Marital Status</label>
                  <select
                    className="form-control"
                    name="maritalStatus"
                    value={editProfile.maritalStatus}
                    onChange={handleInputChange}
                  >
                    <option value="single">Single</option>
                    <option value="married">Married</option>
                    <option value="divorced">Divorced</option>
                    <option value="widowed">Widowed</option>
                  </select>
                </div>
                <div className="col-6">
                  <label>Languages</label>
                  <MultiSelect
                    options={options}
                    value={selected}
                    onChange={setSelected}
                    labelledBy="Select"
                  />
                </div>
              </div>
              <div className="row mt-3">
                <div className="col-6 d-flex align-items-center">
                  <label className="mr-3">Gender:</label>
                  <div className="form-check mr-3">
                    <input
                      className="form-check-input"
                      type="radio"
                      id="male"
                      name="gender"
                      value="male"
                      checked={editProfile.gender === "male"}
                      onChange={handleInputChange}
                    />
                    <label className="form-check-label" htmlFor="male">
                      Male
                    </label>
                  </div>
                  <div className="form-check">
                    <input
                      className="form-check-input"
                      type="radio"
                      id="female"
                      name="gender"
                      value="female"
                      checked={editProfile.gender === "female"}
                      onChange={handleInputChange}
                    />
                    <label className="form-check-label" htmlFor="female">
                      Female
                    </label>
                  </div>
                </div>

                <div className="col-6">
                  <label>Race</label>
                  <select
                    className="form-control"
                    name="race"
                    value={editProfile.race}
                    onChange={handleInputChange}
                  >
                    {allRaces.map((race) => (
                      <option key={race.id} value={race.name}>
                        {race.name}
                      </option>
                    ))}
                  </select>
                </div>
              </div>
              <div className="row mt-3">
                <div className="d-flex gap-5 mt-2 justify-content-left">
                  <button type="submit" className="btn btn-primary">
                    Save
                  </button>
                  <button
                    type="button"
                    className="btn btn-secondary"
                    onClick={() => setShowEditForm(false)}
                  >
                    Cancel
                  </button>
                </div>
              </div>
            </form>
          )}
        </Modal.Body>
      </Modal>

      <ConfirmationModal
        show={showModal}
        handleClose={() => setShowModal(false)}
        handleConfirm={handleConfirmDelete}
      />
    </div>
  );
}

export default CustomTableContainer;
