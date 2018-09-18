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

                treeArr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });

                $('#treeProductCategory').tree({
                    data: treeArr,
                    dnd: true,
                    onDrop: function (target, source, point) {
                        var targetNode = $(this).tree('getNode', target);
                        if (point === 'append') {
                            var children = [];
                            $.each(targetNode.children,
                                function (i, item) {
                                    children.push({
                                        key: item.id,
                                        value: i
                                    });
                                });
                            //Update Db
                            $.ajax({
                                url:'/admin/ProductCategory/UpdateParentId',
                                type:'POST',
                                data: {
                                    sourceId:source.id,
                                    targetId:targetNode.id,
                                    items:children
                                },
                                success: function(response) {
                                    getProductCategory();
                                }
                            });
                        } else if (point==='top'|| point==='bottom') {
                            $.ajax({
                                url:'/admin/ProductCategory/UpdateOrderId',
                                type:'POST',
                                data: {
                                    sourceId:source.id,
                                    targetId:targetNode.id
                                },
                                success: function(response) {
                                    getProductCategory();
                                }
                            });
                        }
                    }
                });
            },
            error: function (error) {
                app.notify(error, 'error');
            }
        });
    };
}