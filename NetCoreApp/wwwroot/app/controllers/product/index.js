var productController = function() {
    this.initialize = function() {
        loadData();
    };

    function registerEvent() {

    }

    function loadData() {
        var template = $('#table-template-product').html();
        var render = "";
        $.ajax({
            type: 'GET',
            url: '/admin/product/GetAll',
            dataType: 'json',
            success: function(response) {
                if (response.length == 0) {
                    app.notify('No data', 'error');
                } else {
                    $.each(response,
                        function(i, item) {
                            render += Mustache.render(template,
                                {
                                    Id: item.Id,
                                    Name: item.Name,
                                    Category: item.ProductCategory.Name,
                                    Price: item.Price,
                                    Image: item.Image ='<img src="/vendor/gentelella/production/user.png" width = "25px"></img>',
                                        //? '<img src="' + item.Image + '"width="25px"></img>'
                                        //: '<img src="~/vendor/gentelella/production/user.png"></img>',
                                    CreateDay: app.dateFormatJson(item.DateCreated),
                                    Status: app.getStatus(item.Status)
                                });
                            if (render !== "") {
                                $('#table-content').html(render);
                            } else {
                                app.notify('Count load data', 'error');
                            }

                        });
                }

            },
            error: function(error) {
                console.log(error);
                app.notify('Can not Load Product', 'error');
            }
        });
    }
};

