﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainPage.aspx.cs" Inherits="Listing_Searcher.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Property Listings</title>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>


    <script type="text/javascript">
        function openCoDetail() {
            $('#modCoDetail').modal('show');
        }
    </script>


    <script type="text/javascript">
        function copyToClipboard(listingData) {
            let textToCopy = '';
            if(listingData.listingDetails[0]) {
                textToCopy += 'Name: ' + listingData.listingDetails[0] + '\n';
            }
            if (listingData.listingDetails[1]) {
                textToCopy += 'Type: ' + listingData.listingDetails[1] + '\n';
            }
            if (listingData.listingDetails[2]) {
                textToCopy += 'Build Up: ' + listingData.listingDetails[2] + '\n';
            }
            if (listingData.listingDetails[3]) {
                textToCopy += 'Furnishing: ' + listingData.listingDetails[3] + '\n';
            }
            if (listingData.listingDetails[4]) {
                textToCopy += 'Bedrooms: ' + listingData.listingDetails[4] + '\n';
            }
            if (listingData.listingDetails[5]) {
                textToCopy += 'Bathrooms: ' + listingData.listingDetails[5] + '\n';
            }
            if (listingData.listingDetails[6]) {
                textToCopy += 'Carpark: ' + listingData.listingDetails[6] + '\n';
            }
            if (listingData.listingDetails[7]) {
                textToCopy += 'Sale or Rent: ' + listingData.listingDetails[7] + '\n';
            }
            if (listingData.listingDetails[8]) {
                textToCopy += 'Price: ' + listingData.listingDetails[8] + '\n\n';
            }

            if (listingData.Photos && listingData.Photos.length > 0) {
                textToCopy += 'Photos:\n';
                listingData.Photos.forEach(function (photo, index) {
                    textToCopy += photo + '\n';
                });
            }

            var tempTextArea = document.createElement("textarea");
            tempTextArea.value = textToCopy;
            document.body.appendChild(tempTextArea);
            tempTextArea.select();

            navigator.clipboard.writeText(tempTextArea.value)
                .then(() => {
                    alert('Property details and photos copied to clipboard!');
                })
                .catch(err => {
                    console.error('Could not copy text: ', err);
                });

            document.body.removeChild(tempTextArea);
        }
    </script>

    <style>
        .checkbox-spacing{
            margin-left:2px;
        }
    </style>

    <style>
        .centered th{
            text-align: center !important;
        }
        
        .darker-grid td,
        .darker-grid th,
        .darker-grid tr{
            border:2px solid #bfbfbf
        }
    </style>

    <style>
        .button-spacing{
            margin-bottom:5px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-left: 30px; margin-right: 30px;">

            <div class="row">
                <div class="col-xs-12">
                    <h1>Add New Property Listings</h1>
                </div>
            </div>

            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-4">
                    <asp:Label ID="lblPropertyName" runat="server" Text="Property Name: " Font-Size="17px"></asp:Label>
                    <asp:TextBox ID="txtbxPropertyName" runat="server" Font-Size="17px"></asp:TextBox>
                </div>
            </div>

            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-4">
                    <asp:Label ID="lblPropertyType" runat="server" Text="Property Type: " Font-Size="17px"></asp:Label>
                    <asp:TextBox ID="txtbxPropertyType" runat="server" Font-Size="17px"></asp:TextBox>
                </div>
            </div>

            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-4">
                    <asp:Label ID="lblAddress" runat="server" Text="Address: " Font-Size="17px"></asp:Label>
                    <asp:TextBox ID="txtbxAddress" runat="server" Font-Size="17px"></asp:TextBox>
                </div>
            </div>

            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-4">
                    <asp:Label ID="lblUnitNumber" runat="server" Text="Unit Number: " Font-Size="17px"></asp:Label>
                    <asp:TextBox ID="txtbxUnitNumber" runat="server" Font-Size="17px"></asp:TextBox>
                </div>
            </div>

            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-4">
                    <asp:Label ID="lblBuildUp" runat="server" Text="Build Up (sqft): " Font-Size="17px"></asp:Label>
                    <asp:TextBox ID="txtbxBuildUp" runat="server" Font-Size="17px"></asp:TextBox>
                </div>
            </div>

            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-4">
                    <asp:Label ID="lblFurnishing" runat="server" Text="Furnishing: " Font-Size="17px"></asp:Label>
                    <asp:TextBox ID="txtbxFurnishing" runat="server" Font-Size="17px"></asp:TextBox>
                </div>
            </div>

            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-4">
                    <asp:Label ID="lblNumOfBedroom" runat="server" Text="Number of Bedrooms: " Font-Size="17px"></asp:Label>
                    <asp:TextBox ID="txtbxNumOfBedroom" runat="server" Font-Size="17px"></asp:TextBox>
                </div>
            </div>

            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-4">
                    <asp:Label ID="lblNumOfBathroom" runat="server" Text="Number of Bathrooms: " Font-Size="17px"></asp:Label>
                    <asp:TextBox ID="txtbxNumOfBathroom" runat="server" Font-Size="17px"></asp:TextBox>
                </div>
            </div>

            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-4">
                    <asp:Label ID="lblNumOfCarpark" runat="server" Text="Number of Carparks: " Font-Size="17px"></asp:Label>
                    <asp:TextBox ID="txtbxNumOfCarpark" runat="server" Font-Size="17px"></asp:TextBox>
                </div>
            </div>

            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-4">
                    <asp:Label ID="lblSaleOrRent" runat="server" Text="Sale/Rent: " Font-Size="17px"></asp:Label>
                    <asp:CheckBox ID="chkbxSale" runat="server" Text="Sale" Font-Size="17px" Checked="false" CssClass="checkbox-spacing" />
                    <asp:CheckBox ID="chkbxRent" runat="server" Text="Rent" Font-Size="17px" Checked="false" CssClass="checkbox-spacing" />
                </div>
            </div>

            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-4">
                    <asp:Label ID="lblPrice" runat="server" Text="Price: RM" Font-Size="17px"></asp:Label>
                    <asp:TextBox ID="txtbxPrice" runat="server" Font-Size="17px"></asp:TextBox>
                </div>
            </div>

            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-4">
                    <asp:Label ID="lblOwnerPhoneNum" runat="server" Text="Owner Phone Number: " Font-Size="17px"></asp:Label>
                    <asp:TextBox ID="txtbxOwnerPhoneNum" runat="server" Font-Size="17px"></asp:TextBox>
                </div>
            </div>

            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-4">
                    <asp:Label ID="lblPosted" runat="server" Text="Posted " Font-Size="17px"></asp:Label>
                    <asp:CheckBox ID="chkbxPosted" runat="server" Checked="false" />
                </div>
            </div>

            <div class="row" style="margin-bottom: 30px;">
                <div class="col-sm-4">
                    <asp:Label ID="lblPhoto" runat="server" Text="Photo: " Font-Size="17px"></asp:Label>
                    <asp:FileUpload ID="fileupPhoto" runat="server"   AllowMultiple="true"/>
                </div>
            </div>

            <div class="row" style="margin-bottom: 50px;">
                <div class="col-sm-4">
                    <asp:Button ID="btnAddListing" runat="server" Text="Add New Listing" font-size="Large" OnClick="btnAddListing_Click" CssClass="btn btn-success"/>
                </div>
            </div>

            <div class="row">
                <div class="col-xs-12">
                    <h1>Search and View Property Listings</h1>
                </div>
            </div>

            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-4">
                    <asp:DropDownList ID="drpdwnSearch" runat="server" Font-Size="17px" ClientIDMode="Static" onchange="toggleTextboxes()">
                        <asp:ListItem Text="-- Select a Search Option --" Value=""></asp:ListItem>
                        <asp:ListItem Text="Property Name" Value="PropertyName"></asp:ListItem>
                        <asp:ListItem Text="Property Type" Value="PropertyType"></asp:ListItem>
                        <asp:ListItem Text="Address" Value="Address"></asp:ListItem>
                        <asp:ListItem Text="Build Up" Value="BuildUp"></asp:ListItem>
                        <asp:ListItem Text="Furnishing" Value="Furnishing"></asp:ListItem>
                        <asp:ListItem Text="Number of Bedroom" Value="NumOfBedroom"></asp:ListItem>
                        <asp:ListItem Text="Number of Bathroom" Value="NumOfBathroom"></asp:ListItem>
                        <asp:ListItem Text="Number of Carpark" Value="NumOfCarpark"></asp:ListItem>
                        <asp:ListItem Text="Sale or Rent" Value="SaleOrRent"></asp:ListItem>
                        <asp:ListItem Text="Price" Value="Price"></asp:ListItem>
                        <asp:ListItem Text="Status" Value="Status"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row" style="margin-bottom: 15px;">
                <div class="col-sm-4">
                    <asp:TextBox ID="txtbxSearch1" runat="server" Font-Size="17px" ClientIDMode="Static" style="display:none;"></asp:TextBox>
                    <asp:TextBox ID="txtbxSearch2" runat="server" Font-Size="17px" ClientIDMode="Static" style="display:none;"></asp:TextBox>
                </div>
            </div>

            <script type="text/javascript">
                function toggleTextboxes() {
                    var dropdown = document.getElementById('drpdwnSearch');
                    var selectedValue = dropdown.options[dropdown.selectedIndex].value;

                    var txtbx1 = document.getElementById('txtbxSearch1');
                    var txtbx2 = document.getElementById('txtbxSearch2');

                    txtbx1.style.display = 'none';
                    txtbx2.style.display = 'none';

                    if (selectedValue === 'BuildUp' || selectedValue === 'Price') {
                        txtbx1.style.display = 'block';
                        txtbx2.style.display = 'block';
                    } else if (selectedValue === 'PropertyName' || selectedValue === 'PropertyType' || selectedValue === 'Address' || selectedValue === 'Furnishing' || selectedValue === 'NumOfBedroom' || selectedValue === 'NumOfBathroom' || selectedValue === 'NumOfCarpark' || selectedValue === 'SaleOrRent' || selectedValue === 'Status') {
                        txtbx1.style.display = 'block';
                    }
                }
            </script>

            <div class="row" style="margin-bottom:15px;">
                <div class="col-sm-4">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" Font-Size="Large" OnClick="btnSearch_Click"/>
                </div>
            </div>

            <asp:GridView ID="gvPropertyListings" runat="server" AutoGenerateColumns="false" DataKeyNames="ListingID" CssClass="table table-bordered; centered; darker-grid" OnRowDataBound="gvPropertyListings_RowDataBound" OnRowEditing="gvPropertyListings_RowEditing" OnRowUpdating="gvPropertyListings_RowUpdating" OnRowCancelingEdit="gvPropertyListings_RowCancelingEdit" OnRowDeleting="gvPropertyListings_RowDelete" OnRowCommand="gvPropertyListings_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Property Name">
                        <ItemTemplate>
                            <%# Eval("PropertyName") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPropertyName" runat="server" Text='<%# Bind("PropertyName") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Property Type">
                        <ItemTemplate>
                            <%# Eval("PropertyType") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPropertyType" runat="server" Text='<%# Bind("PropertyType") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Address">
                        <ItemTemplate>
                            <%# Eval("Address") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtAddress" runat="server" Text='<%# Bind("Address") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit Number">
                        <ItemTemplate>
                            <%# Eval("UnitNum") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtUnitNum" runat="server" Text='<%# Bind("UnitNum") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Build Up">
                        <ItemTemplate>
                            <%# Eval("BuildUp") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtBuildUp" runat="server" Text='<%# Bind("BuildUp") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Furnishing">
                        <ItemTemplate>
                            <%# Eval("Furnishing") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtFurnishing" runat="server" Text='<%# Bind("Furnishing") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Number of Bedroom">
                        <ItemTemplate>
                            <%# Eval("NumOfBedroom") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtNumOfBedroom" runat="server" Text='<%# Bind("NumOfBedroom") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Number of Bathroom">
                        <ItemTemplate>
                            <%# Eval("NumOfBathroom") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtNumOfBathroom" runat="server" Text='<%# Bind("NumOfBathroom") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Number of Carpark">
                        <ItemTemplate>
                            <%# Eval("NumOfCarpark") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtNumOfCarpark" runat="server" Text='<%# Bind("NumOfCarpark") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sale or Rent">
                        <ItemTemplate>
                            <%# Eval("SaleOrRent") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtSaleOrRent" runat="server" Text='<%# Bind("SaleOrRent") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Price">
                        <ItemTemplate>
                            <%# Eval("Price") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPrice" runat="server" Text='<%# Bind("Price") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Owner Phone Number">
                        <ItemTemplate>
                            <%# Eval("OwnerPhoneNum") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtOwnerPhoneNum" runat="server" Text='<%# Bind("OwnerPhoneNum") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <%# Eval("Status") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtStatus" runat="server" Text='<%# Bind("Status") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Posted" Visible="false" />
                    <asp:TemplateField HeaderText="Photos" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button ID="btnViewPhotos" runat="server" Text="View Photos" CommandArgument='<%# Eval("ListingID") %>' OnClick="btnViewPhotos_Click" CssClass="btn btn-info" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CommandArguments='<%# Container.DataItemIndex %>' Text="Edit" Width="66px" CssClass="btn btn-warning button-spacing" />
                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArguments='<%# Eval("ListingID") %>' Text="Delete" Width="66px" CssClass="btn btn-danger button-spacing"  OnClientClick="return confirm('Are you sure you want to delete this listing?');" />
                            <asp:Button ID="btnPosted" runat="server" CommandName="Posted" CommandArgument='<%# Eval ("ListingID") %>' Text="Posted" Width="66px" CssClass="btn btn-success button-spacing" Visible='<%#Eval("Posted").ToString() == "0" %>' OnClientClick="return confirm('Are you sure you have posted this listing?');" />
                            <asp:Button ID="btnCopy" runat="server" CommandName="Copy" CommandArgument='<%#Eval("ListingID") %>' Text="Copy" Width="66px" CssClass="btn btn-primary" OnClientClick="return false;" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Button ID="btnUpdate" runat="server" CommandName="Update" CommandArguments='<%# Eval("ListingID") %>' Text="Update" Width="66px" CssClass="btn update-btn btn-success button-spacing" />
                            <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" CommandArguments='<%# Eval("ListingID") %>' Text="Cancel" Width="66px" CssClass="btn cancel-btn btn-danger" />
                        </EditItemTemplate> 
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <div id="photoModal" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h4 class="modal-title">Property Photos</h4>
                        </div>
                        <div class="modal-body">
                            <asp:Repeater ID="rptPhotos" runat="server">
                                <ItemTemplate>
                                    <img src='<%# "data:image/png;base64," + Convert.ToBase64String((byte[])Eval("Photo")) %>' 
                                         alt="Property Photo" class="img-thumbnail" style="width: 100%; margin-bottom: 10px;" />
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </form>
</body>
</html>