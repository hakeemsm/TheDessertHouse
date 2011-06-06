$(function () {
    $('#newRole').val('');
    $("#btnAddRole").click(function () {
        var rolename = $('#newRole').val();
        $.ajax({
            type: "post",
            url: "/Account/CreateRole?newRole=" + rolename,
            dataType: 'json',
            success: function (response) {
                var newRow = [{ newRole: response.roleName}];
                $('#newRoleRowTemplate').tmpl(newRow).appendTo('#tblRoles').slideDown("slow");
                var numRoles = $("#roleCount").text() * 1;
                $("#roleCount").text(numRoles + 1);
                $('#newRole').val('');
            },
            error: function (x, status, ex) {
                alert("An error occurred creating the role. Please try again later");
            }
        })
    })
    //$('a.delete-role-button').live('each',function () {
        $('a.delete-role-button').live('click',function () {
            var roleId = $(this).attr('meta:id');
            $.ajax({
                url: deleteRoleUrl,
                data: { role: roleId },
                type: "post",
                success: function (response) {
                    if (response.roleDeleted) {
                        $('tr#role-' + response.roleId).remove();
                        $("#roleCount").text(response.roleCount);
                        $('#newRole').val('');
                    }
                    else {
                        alert("The role " + response.roleId + " could not be deleted at this time. Please try again later");
                    }
                }

            })


        })
    //})
})    
    