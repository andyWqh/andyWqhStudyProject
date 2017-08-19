<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="jqTmplDemo.aspx.cs" Inherits="andyWqhWeb.jqueryTmpl.jqTmplDemo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>jqTmplDemo</title>
    <script src="../Scripts/jquery/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../Scripts/jQueryTmpl/jquery.tmpl.js" type="text/javascript"></script>
    <style type="text/css">
        .tab
        {
            border-collapse: collapse;
            margin-left:20px;
        }
        
        .tab tr td
        {
            padding: 10px;
            text-align: center;
            width: 80px;
        }
    </style>
    <!--定义jq tmpl 模板-->
    <!--基本访问语法-->
    <script type="text/x-jquery-tmpl" id="tabDemoTemplate">
        <tr>
            <td>ID:</td>
            <td>${ID}</td>
            <td>Name:</td>
            <td>{{= Name}}</td>
            <td>Num:</td>
            <td>${Number(Num)+1}</td>
            <td>Status:</td>
            <td>${Status}</td>
        </tr>
    </script>

    <!--缓存template() 开始-->
    <script type="text/x-jquery-tmpl" id="cacheDemo">
         {{tmpl 'cached'}}
        <tr>
            <td>${ID}</td>
            <td>${Name}</td>
        </tr>
    </script>
    <script type="text/x-jquery-tmpl" id="cache">
        <tr>
            <td colspan="2">${Group}</td>
        </tr>
    </script>
     <!--缓存template() 结束-->


    <!--{{each}}方法提供循环遍历逻辑-->
    <script type="text/x-jquery-tmpl" id="eachDemo">
        <h3>users</h3>
         <span>userInfo:</span>
        {{each(i,user) users}}
            <div>${i+1} &nbsp;
            {{= user.name}}</div>
            {{if i==0}}
                <h4>group</h4>
                <span>user_groupinfo:</span>
                {{each(j,group) groups}}
                   <div>
                   ${j+1} &nbsp;
                   <span>name:</span>
                   ${group.name}</div>
                {{/each}}
            {{else}}
                <h4>depart</h4>
                <sapn>departinfo:</span>
                {{each departs}}
                    <div><span>name:</span>{{= $value.name}}</div>
                {{/each}}
            {{/if}}
        {{/each}}
        <h3>depart</h3>
        <sapn>departinfo:</span>
        {{each departs}}
            <div><span>name:</span>{{= $value.name}}</div>
        {{/each}}
    </script>

    <!--{{if}} {{else}} 分支语法-->
    <script type="text/x-jquery-tmpl" id="ifesleDemo">
         <tr style="margin-bottom:10px;">
            <td>${ID}</td>
            <td>{{= Name}}</td>
            <td>
                {{if Status}}
                    <span>Status:${Status}</span>
                {{else App}}
                    <span>App:${App}</span>
                {{else}}
                    <span>None</span>
                {{/if}}
            </td>
         </tr>
    </script>

    <!--{{= html}} ${html} 输出变量html值-->
    <script type="text/x-jquery-tmpl" id="htmlDemo">
        <div style="margin-bottom:10px;">
　　　　    <span>${ID}</span>
　　　　    <span style="margin-left:10px;">{{= Name}}</span>
    　　    ${html}
    　　    {{html html}}
    </div>
    </script>

    <!--{{tmpl}} 嵌套模板 开始-->
    <script id="tmpl1" type="text/x-jquery-tmpl">
        <div style="margin-bottom:10px;">
    　　    <span>${ID}</span>
    　　    <span style="margin-left:10px;">{{tmpl($data) '#tmpl2'}}</span>
        </div>     
   </script>

   <script id="tmpl2" type="type/x-jquery-tmpl">
        {{each Name}} <span style="color:red">${$value}</span>{{/each}}   
   </script> 
   <!--{{tmpl}} 嵌套模板 结束-->

   <!--$data,$item实现-->
   <script id="item_data" type="text/x-jquery-tmpl"> 
     <div style="margin-bottom:10px;">
　　　　<span>${$data.ID}</span>
　　　　<span style="margin-left:10px;">${$item.getName("==")}</span>
　　　</div>
</script> 


    <!--jq脚本-->
    <script type="text/javascript">
        $(function () {

            //基本用法 ${}等同与{{=}}是输出变量 ${}里面还可以放表达式 （=和变量之间一定要有空格，否则无效）
            var users = [
                            { ID: 'think8848', Name: 'Joseph Chan', Num: '1', Status: 1 },
                            { ID: 'aCloud', Name: 'Mary Cheung', Num: '2', Status: 0 }
                        ];

            $("#tabDemoTemplate").tmpl(users).appendTo("#tabDemo");

            //测试缓存template()
            var groupUsers = [
                                { ID: 'think8848', Name: 'Joseph Chan', Group: 'Administrators' },
                                { ID: 'aCloud', Name: 'Mary Cheung', Group: 'Users' }
                             ];
            $("#cache").template("cached");
            $("#cacheDemo").tmpl(groupUsers).appendTo('#tabCacheDemo');

            //{{each}}方法提供循环遍历逻辑，$value访问迭代变量 也可以自定义迭代变量(i,value)
            var eachData = {
                users: [{ name: 'jerry' }, { name: 'john'}],
                groups: [{ name: 'mingdao' }, { name: 'meihua' }, { name: 'test'}],
                departs: [{ name: 'IT'}]
            };

            $("#eachDemo").tmpl(eachData).appendTo('#eachDivDemo');

            //{{if}} {{else}} 分支语法
            var usersInfo = [
                                { ID: 'think8848', Name: 'Joseph Chan', Status: 1, App: 0 },
                                { ID: 'aCloud', Name: 'Mary Cheung', App: 1 },
                                { ID: 'bMingdao', Name: 'Jerry Jin' }
                            ];
            $("#ifesleDemo").tmpl(usersInfo).appendTo('#tab_if_else');

            //{{= html}} ${html} 输出变量html值，没有html编码，适合输出html代码 ；{{html html}}是输出HTML编码渲染后的值
            var userHtml = { ID: 'think8848', Name: 'Joseph Chan', html: '<button>html</button>' };
            $("#htmlDemo").tmpl(userHtml).appendTo('#div_html');

            //{{tmpl}} 嵌套模板，$value当前访问迭代变量,$data当前访问数据，$item当前访问模板
            var usersTmpl = [{ ID: 'think8848', Name: ['Joseph', 'Chan', 'wqh'] }, { ID: 'aCloud', Name: ['Mary', 'Cheung', 'andy']}];
            $("#tmpl1").tmpl(usersTmpl).appendTo('#tmpl');

            //template()方法使用
            var markup = '<tr><td>${ID}</td><td>${Name}</td></tr>';
            $.template('template', markup);
            $.tmpl('template', usersInfo).appendTo('#templateRows');

            //$data,$item实现
            var users = [{ ID: 'think8848', Name: ['Joseph', 'Chan'] }, { ID: 'aCloud', Name: ['Mary', 'Cheung']}];
            $("#item_data").tmpl(users,
                {
                    getName: function (spr) {
                        return this.data.Name.join(spr);
                    }
                }
            ).appendTo('#div_item_data');

            //$.tmplItem()方法，使用这个方法，可以获取从render出来的元素上重新获取$item

                $('table').delegate('tr', 'click', function () {
                    var item = $.tmplItem(this);
                    alert(item.data.Name);
                });
        })
    </script>
</head>
<body>
    <div id="divDemo">
        <table border="1" cellpadding="0" cellspacing="0" class="tab">
            <thead>
                <tr>
                    <td>
                        ID
                    </td>
                    <td>
                        Name
                    </td>
                </tr>
            </thead>
            <tbody id="tabCacheDemo">
            </tbody>
        </table>
    </div>
    <br />

    <table id="tabDemo" cellpadding="0" cellspacing="0" border="1" class="tab">
    </table>
    <br />

    <div id="eachDivDemo" style=" margin-left:10px; background-color:#999;">
    </div>
    <br />
    <table cellpadding="0" cellspacing="0" border="1" class="tab">
       <thead>
            <tr>
                <td>ID</td>
                <td>Name</td>
                <td>RemarkInfo</td>
            </tr>
       </thead>
       <tbody id="tab_if_else"></tbody>
    </table>
    <br />
    <div id="div_html"></div>
    <br />
    <div id="tmpl"></div>
    <br />
    <div id="templateRows">
    </div>
    <br />
    <div id="div_item_data"></div>
</body>

</html>
