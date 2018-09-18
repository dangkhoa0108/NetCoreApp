var productCategoryController = function () {
    this.initialize = function() {
        getProductCategory();
    };

    function getProductCategory() {
        $.ajax({
            type: 'GET',
            url: '/admin/productcategory/GetProductCategory',
            dataType: 'json',
            success: function (response) {
                var data = [];
                $.each(response,
                    function (i, item) {
                        data.push({
                            id: item.Id,
                            text: item.Name,
                            parentId: item.ParentId,
                            sortOrder: item.SortOrder
                        });
                    });
                var treeArr = app.unflattened(data);
                $('#treeProductCategory').tree({
                    data: treeArr
                });
            },
            error: function (error) {
                app.notify(error, 'error');
            }
        });
    };
}