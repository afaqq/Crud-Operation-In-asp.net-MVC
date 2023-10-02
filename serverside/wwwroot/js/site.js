function openPopup(link, url, title) {
    var studentId = $(link).data("student-id");

    if (studentId !== undefined) {
        
        $.ajax({
            type: "GET",
            url: url,
            success: function (res) {
                $("#form-modal .modal-body").html(res);
                $("#form-modal .modal-title").html(title);
                $("#form-modal").modal('show');

               
                submitAcademicRecordForm(url);
            },
            error: function () {
                alert("An error occurred while loading the page.");
            }
        });
    } else {
        alert("Student ID is undefined.");
    }
}


// Function to handle form submission via AJAX
function submitAcademicRecordForm(url) {
    $("#academicRecordForm").submit(function (event) {
        event.preventDefault();

        var formData = $(this).serialize();

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            success: function (response) {
                
                $("#Table tbody").html(response);
                $("#form-modal").modal('hide'); 
            },
            error: function () {
                alert("An error occurred while adding the academic record.");
            }
        });
    });
}

 function deleteAcademicRecord(academicId) {
        if (confirm("Are you sure you want to delete this academic record?")) {
            $.ajax({
                type: "POST",
                url: "/Student/DeleteAcademicRecord", 
                data: { id: academicId }, 
                dataType: "json",
                success: function (result) {
                    if (result.success) {
                        
                        location.reload();
                    } else {
                        alert("Error deleting academic record: " + result.message);
                    }
                },
                error: function () {
                    alert("An error occurred while deleting the academic record.");
                }
            });
        }
    }
 





