using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Listing_Searcher
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        SqlConnection myCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ListingDatabase"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPropertyListings();
            }
        }

        protected void btnAddListing_Click(object sender, EventArgs e)
        {
            try
            {
                myCon.Open();
                string insertListingQuery = @"INSERT INTO PropertyListings (PropertyName, PropertyType, Address, UnitNum, BuildUp, Furnishing, NumOfBedroom, NumOfBathroom, NumOfCarpark, SaleOrRent, Price, OwnerPhoneNum, Posted) VALUES (@PropertyName, @PropertyType, @Address, @UnitNum, @BuildUp, @Furnishing, @NumOfBedroom, @NumOfBathroom, @NumOfCarpark, @SaleOrRent, @Price, @OwnerPhoneNum, @Posted); SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(insertListingQuery, myCon);
                cmd.Parameters.AddWithValue("@PropertyName", txtbxPropertyName.Text);
                cmd.Parameters.AddWithValue("@PropertyType", txtbxPropertyType.Text);
                cmd.Parameters.AddWithValue("@Address", txtbxAddress.Text);
                cmd.Parameters.AddWithValue("@UnitNum", txtbxUnitNumber.Text);
                
                if (int.TryParse(txtbxBuildUp.Text, out int buildUp))
                {
                    cmd.Parameters.AddWithValue("@BuildUp", buildUp);
                }
                else if (txtbxBuildUp.Text == "")
                {
                    cmd.Parameters.AddWithValue("@BuildUp", buildUp);
                }
                else
                {
                    string script = "alert('Please enter a valid Build Up.');";
                    ClientScript.RegisterStartupScript(this.GetType(), "Error", script, true);
                    txtbxBuildUp.Text = "";
                    return;
                }
                
                cmd.Parameters.AddWithValue("@Furnishing", txtbxFurnishing.Text);
                
                if (int.TryParse(txtbxNumOfBedroom.Text, out int numOfBedroom))
                {
                    cmd.Parameters.AddWithValue("@NumOfBedroom", numOfBedroom);
                }
                else if (txtbxNumOfBedroom.Text == "")
                {
                    cmd.Parameters.AddWithValue("@NumOfBedroom", numOfBedroom);
                }
                else
                {
                    string script = "alert('Please enter a valid Number for Bedrooms.');";
                    ClientScript.RegisterStartupScript(this.GetType(), "Error", script, true);
                    txtbxNumOfBedroom.Text = "";
                    return;
                }
                
                if (int.TryParse(txtbxNumOfBathroom.Text, out int numOfBathroom))
                {
                    cmd.Parameters.AddWithValue("@NumOfBathroom", numOfBathroom);
                }
                else if (txtbxNumOfBathroom.Text == "")
                {
                    cmd.Parameters.AddWithValue("@NumOfBathroom", numOfBathroom);
                }
                else
                {
                    string script = "alert('Please enter a valid Number for Bathrooms.');";
                    ClientScript.RegisterStartupScript(this.GetType(), "Error", script, true);
                    txtbxNumOfBathroom.Text = "";
                    return;
                }
                
                if (int.TryParse(txtbxNumOfCarpark.Text, out int numOfCarpark))
                {
                    cmd.Parameters.AddWithValue("@NumOfCarpark", numOfCarpark);
                }
                else if (txtbxNumOfCarpark.Text == "")
                {
                    cmd.Parameters.AddWithValue("@NumOfCarpark", numOfCarpark);
                }
                else
                {
                    string script = "alert('Please enter a valid Number for Carparks.');";
                    ClientScript.RegisterStartupScript(this.GetType(), "Error", script, true);
                    txtbxNumOfCarpark.Text = "";
                    return;
                }
                
                if (chkbxSale.Checked && !chkbxRent.Checked)
                {
                    cmd.Parameters.AddWithValue("@SaleOrRent", "Sale");
                }
                else if (!chkbxSale.Checked && chkbxRent.Checked)
                {
                    cmd.Parameters.AddWithValue("@SaleOrRent", "Rent");
                }
                else if (chkbxSale.Checked && chkbxRent.Checked)
                {
                    cmd.Parameters.AddWithValue("@SaleOrRent", "Sale & Rent");
                }
                else
                {
                    string script = "alert('Please choose at least one.');";
                    ClientScript.RegisterStartupScript(this.GetType(), "Error", script, true);
                    txtbxPrice.Text = "";
                    return;
                }
                
                if (int.TryParse(txtbxPrice.Text, out int price))
                {
                    cmd.Parameters.AddWithValue("@Price", price);
                }
                else if (txtbxPrice.Text == "")
                {
                    cmd.Parameters.AddWithValue("@Price", price);
                }
                else
                {
                    string script = "alert('Please enter a valid Price.');";
                    ClientScript.RegisterStartupScript(this.GetType(), "Error", script, true);
                    txtbxPrice.Text = "";
                    return;
                }
                
                cmd.Parameters.AddWithValue("@OwnerPhoneNum", txtbxOwnerPhoneNum.Text);
                cmd.Parameters.AddWithValue("@Posted", chkbxPosted.Checked);
                

                int newListingID = Convert.ToInt32(cmd.ExecuteScalar());

                if (fileupPhoto.HasFile && fileupPhoto.PostedFiles.Count > 0)
                {
                    foreach (HttpPostedFile postedFile in fileupPhoto.PostedFiles)
                    {
                        byte[] photoData;
                        using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                        {
                            photoData = br.ReadBytes(postedFile.ContentLength);
                        }
                        string insertPhotoQuery = "INSERT INTO PropertyPhotos (ListingID, Photo) VALUES (@ListingID, @Photo)";
                        SqlCommand photoCmd = new SqlCommand(insertPhotoQuery, myCon);
                        photoCmd.Parameters.AddWithValue("@ListingID", newListingID);
                        photoCmd.Parameters.AddWithValue("@Photo", photoData);
                        photoCmd.ExecuteNonQuery();
                    }
                }
                ClearInputFields();

            }
            catch (Exception ex)
            {
                string script = "alert('Error: " + ex.Message.Replace("'", "\\'") + "');";
                ClientScript.RegisterStartupScript(this.GetType(), "Error", script, true);
                ClearInputFields();
            }
            finally
            {
                myCon.Close();
                BindPropertyListings();
            }
        }

        private void ClearInputFields()
        {
            txtbxPropertyName.Text = string.Empty;
            txtbxPropertyType.Text = string.Empty;
            txtbxAddress.Text = string.Empty;
            txtbxUnitNumber.Text = string.Empty;
            txtbxBuildUp.Text = string.Empty;
            txtbxFurnishing.Text = string.Empty;
            txtbxNumOfBedroom.Text = string.Empty;
            txtbxNumOfBathroom.Text = string.Empty;
            txtbxNumOfCarpark.Text = string.Empty;
            chkbxSale.Checked = false;
            chkbxRent.Checked = false;
            txtbxPrice.Text = string.Empty;
            txtbxOwnerPhoneNum.Text = string.Empty;
            chkbxPosted.Checked = false;
            fileupPhoto.Attributes.Clear();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string selectedColumn = drpdwnSearch.SelectedValue;
            string searchValue1 = txtbxSearch1.Text.Trim();
            string searchValue2 = txtbxSearch2.Text.Trim();

            if (!string.IsNullOrEmpty(selectedColumn))
            {
                try
                {
                    myCon.Open();
                    string query = "";

                    if (selectedColumn == "BuildUp" || selectedColumn == "Price")
                    {
                        if (!string.IsNullOrEmpty(searchValue1) && !string.IsNullOrEmpty(searchValue2))
                        {
                            if (int.TryParse(searchValue1, out int value1) && int.TryParse(searchValue2, out int value2))
                            {
                                if (value1 <= value2)
                                {
                                    query = $"SELECT * FROM PropertyListings WHERE {selectedColumn} BETWEEN @value1 AND @value2";
                                }
                                else
                                {
                                    string script = "alert('The first value must be less than or equal to the second value.');";
                                    ClientScript.RegisterStartupScript(this.GetType(), "Error", script, true);
                                    return;
                                }
                            }
                            else
                            {
                                string script = "alert('Please enter valid numeric values.');";
                                ClientScript.RegisterStartupScript(this.GetType(), "Error", script, true);
                                return;
                            }
                        }
                        else
                        {
                            string script = "alert('Please enter both values for a proper range search.');";
                            ClientScript.RegisterStartupScript(this.GetType(), "Error", script, true);
                            return;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(searchValue1))
                        {
                            query = $"SELECT * FROM PropertyListings WHERE {selectedColumn} LIKE @value1";
                        }
                    }

                    SqlCommand cmd = new SqlCommand(query, myCon);
                    if (selectedColumn == "BuildUp" || selectedColumn == "Price")
                    {
                        cmd.Parameters.AddWithValue("@value1", searchValue1);
                        cmd.Parameters.AddWithValue("@value2", searchValue2);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@value1", "%" + searchValue1 + "%");
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    gvPropertyListings.DataSource = dt;
                    gvPropertyListings.DataBind();
                }
                catch (Exception ex)
                {
                    string script = "alert('Error: " + ex.Message.Replace("'", "\\'") + "');";
                    ClientScript.RegisterStartupScript(this.GetType(), "Error", script, true);
                }
                finally
                {
                    myCon.Close();
                    drpdwnSearch.SelectedIndex = -1;
                    txtbxSearch1.Text = string.Empty;
                    txtbxSearch2.Text = string.Empty;
                }
            }
            else
            {
                BindPropertyListings();
                drpdwnSearch.SelectedIndex = -1;
                txtbxSearch1.Text = string.Empty;
                txtbxSearch2.Text = string.Empty;
            }
        }

        private void BindPropertyListings()
        {
            try
            {
                myCon.Open();
                string query = "SELECT * FROM PropertyListings";
                SqlCommand cmd = new SqlCommand(query, myCon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                gvPropertyListings.DataSource = dt;
                gvPropertyListings.DataBind();
            }
            catch (Exception ex)
            {
                string script = "alert('Error: " + ex.Message.Replace("'", "\\'") + "');";
                ClientScript.RegisterStartupScript(this.GetType(), "Error", script, true);
            }
            finally
            {
                myCon.Close();
            }
        }

        protected void gvPropertyListings_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int postedValue = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Posted"));

                if (postedValue == 0)
                {
                    e.Row.BackColor = System.Drawing.Color.FromArgb(255, 236, 218, 242);
                }
            }
        }

        protected void btnViewPhotos_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int listingID = Convert.ToInt32(btn.CommandArgument);
            try
            {
                myCon.Open();
                string query = "SELECT Photo FROM PropertyPhotos WHERE ListingID = @ListingID";
                SqlCommand cmd = new SqlCommand(query, myCon);
                cmd.Parameters.AddWithValue("@ListingID", listingID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dtPhotos = new DataTable();
                da.Fill(dtPhotos);
                rptPhotos.DataSource = dtPhotos;
                rptPhotos.DataBind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#photoModal').modal('show');", true);
            }
            catch (Exception ex)
            {
                string script = "alert('Error: " + ex.Message.Replace("'", "\\'") + "');";
                ClientScript.RegisterStartupScript(this.GetType(), "Error", script, true);
            }
            finally
            {
                myCon.Close();
            }
        }

        protected void gvPropertyListings_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPropertyListings.EditIndex = e.NewEditIndex;
            BindPropertyListings();
        }

        protected void gvPropertyListings_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPropertyListings.EditIndex = -1;
            BindPropertyListings();
        }

        protected void gvPropertyListings_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int listingID = Convert.ToInt32(gvPropertyListings.DataKeys[e.RowIndex].Value);
                GridViewRow row = gvPropertyListings.Rows[e.RowIndex];
                string propertyName = ((TextBox)row.FindControl("txtPropertyName")).Text;
                string propertyType = ((TextBox)row.FindControl("txtPropertyType")).Text;
                string address = ((TextBox)row.FindControl("txtAddress")).Text;
                string unitNum = ((TextBox)row.FindControl("txtUnitNum")).Text;
                if (!int.TryParse(((TextBox)row.FindControl("txtBuildUp")).Text, out int buildUp))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Error", "alert('Please enter a valid number for Build Up.');", true);
                    return;
                }
                string furnishing = ((TextBox)row.FindControl("txtFurnishing")).Text;
                if (!int.TryParse(((TextBox)row.FindControl("txtNumOfBedroom")).Text, out int numOfBedroom))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Error", "alert('Please enter a valid number for Number of Bedroom.');", true);
                    return;
                }
                if (!int.TryParse(((TextBox)row.FindControl("txtNumOfBathroom")).Text, out int numOfBathroom))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Error", "alert('Please enter a valid number for Number of Bathroom.');", true);
                    return;
                }
                if (!int.TryParse(((TextBox)row.FindControl("txtNumOfCarpark")).Text, out int numOfCarpark))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Error", "alert('Please enter a valid number for Number of Carpark.');", true);
                    return;
                }
                string saleOrRent = ((TextBox)row.FindControl("txtSaleOrRent")).Text;
                if (!int.TryParse(((TextBox)row.FindControl("txtPrice")).Text, out int price))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Error", "alert('Please enter a valid number for Price.');", true);
                    return;
                }
                string ownerPhoneNum = ((TextBox)row.FindControl("txtOwnerPhoneNum")).Text;
                string status = ((TextBox)row.FindControl("txtStatus")).Text;

                myCon.Open();
                string updateQuery = @"UPDATE PropertyListings SET PropertyName = @propertyName, PropertyType = @propertyType, Address = @address, UnitNum = @unitNum, BuildUp = @buildUp, Furnishing = @furnishing, NumOfBedroom = @numOfBedroom, NumOfBathroom = @numOfBathroom, NumOfCarpark = @numOfCarpark, SaleOrRent = @saleOrRent, Price = @price, OwnerPhoneNum = @ownerPhoneNum, Status = @status WHERE ListingID = @listingID";
                SqlCommand cmd = new SqlCommand(updateQuery, myCon);
                cmd.Parameters.AddWithValue("@propertyName", propertyName);
                cmd.Parameters.AddWithValue("@propertyType", propertyType);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@unitNum", unitNum);
                cmd.Parameters.AddWithValue("@buildUp", buildUp);
                cmd.Parameters.AddWithValue("@furnishing", furnishing);
                cmd.Parameters.AddWithValue("@numOfBedroom", numOfBedroom);
                cmd.Parameters.AddWithValue("@numOfBathroom", numOfBathroom);
                cmd.Parameters.AddWithValue("@numOfCarpark", numOfCarpark);
                cmd.Parameters.AddWithValue("@saleOrRent", saleOrRent);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@ownerPhoneNum", ownerPhoneNum);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@listingID", listingID);
                cmd.ExecuteNonQuery();
                gvPropertyListings.EditIndex = -1;
            }
            catch (Exception ex)
            {
                string script = "alert('Error: " + ex.Message.Replace("'", "\\'") + "');";
                ClientScript.RegisterStartupScript(this.GetType(), "Error", script, true);
            }
            finally
            {
                myCon.Close();
                BindPropertyListings();
            }
        }

        protected void gvPropertyListings_RowDelete(object sender, GridViewDeleteEventArgs e)
        {
            int listingID = Convert.ToInt32(e.Keys["ListingID"]);
            try
            {
                myCon.Open();
                string query1 = "DELETE FROM PropertyPhotos WHERE ListingID = @ListingID";
                string query2 = "DELETE FROM PropertyListings WHERE ListingID = @ListingID";
                SqlCommand cmd1 = new SqlCommand(query1, myCon);
                SqlCommand cmd2 = new SqlCommand(query2, myCon);
                cmd1.Parameters.AddWithValue("@ListingID", listingID);
                cmd2.Parameters.AddWithValue("@ListingID", listingID);
                cmd1.ExecuteNonQuery();
                int rowsAffected = cmd2.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    string script = "alert('Property listing deleted successfully.');";
                    ClientScript.RegisterStartupScript(this.GetType(), "Success", script, true);
                }
            }
            catch (Exception ex)
            {
                string script = "alert('Error: " + ex.Message.Replace("'", "\\'") + "');";
                ClientScript.RegisterStartupScript(this.GetType(), "Error", script, true);
            }
            finally
            {
                myCon.Close();
                BindPropertyListings();
            }
        }

        protected void gvPropertyListings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Copy")
            {
                int listingID = Convert.ToInt32(e.CommandArgument);
                DataTable dtListing = GetListingDetails(listingID);
                DataTable dtPhotos = GetListingPhotos(listingID);
                string listingData = SerializeListingData(dtListing, dtPhotos);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CopyToClipboard", "copyToClipboard(" + listingData + ");", true);
            }
            else if (e.CommandName == "Posted")
            {
                int listingID = Convert.ToInt32(e.CommandArgument);
                try
                {
                    using (SqlConnection myCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ListingDatabase"].ConnectionString))
                    {
                        myCon.Open();
                        string updateQuery = "UPDATE PropertyListings SET Posted = 1 WHERE ListingID = @listingID";
                        SqlCommand cmd = new SqlCommand(updateQuery, myCon);
                        cmd.Parameters.AddWithValue("@listingID", listingID);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    string script = "alert('Error: " + ex.Message.Replace("'", "\\'") + "');";
                    ClientScript.RegisterStartupScript(this.GetType(), "Error", script, true);
                }
                finally
                {
                    myCon.Close();
                    BindPropertyListings();
                }
            }
        }

        private DataTable GetListingDetails(int listingID)
        {
            DataTable dt = new DataTable();
            using (SqlConnection myCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ListingDatabase"].ConnectionString))
            {
                myCon.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT PropertyName, PropertyType, BuildUp, Furnishing, NumOfBedroom, NumOfBathroom, NumOfCarpark, SaleOrRent, Price FROM PropertyListings WHERE ListingID = @ListingID", myCon))
                {
                    cmd.Parameters.AddWithValue("@ListingID", listingID);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            return dt;
        }

        private DataTable GetListingPhotos(int listingID)
        {
            DataTable dt = new DataTable();
            using (SqlConnection myCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ListingDatabase"].ConnectionString))
            {
                myCon.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Photo FROM PropertyPhotos WHERE ListingID = @ListingID", myCon))
                {
                    cmd.Parameters.AddWithValue("@ListingID", listingID);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            return dt;
        }

        private string SerializeListingData(DataTable dtListing, DataTable dtPhotos)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (dtListing.Rows.Count == 0)
                return js.Serialize(new { ListingDetails = new string[] { }, ListingPhotos = new List<string>() });

            var listingInfo = new
            {
                ListingDetails = dtListing.Rows[0].ItemArray,
                ListingPhotos = dtPhotos.AsEnumerable().Select(row => Convert.ToBase64String((byte[])row["Photo"])).ToList()
            };

            return js.Serialize(listingInfo);
        }

    }
}