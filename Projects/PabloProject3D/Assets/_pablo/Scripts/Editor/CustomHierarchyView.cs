using UnityEngine;
using UnityEditor;
using System.Text;

namespace Pablo
{
  [InitializeOnLoad]
  public class CustomHierarchyView
  {

    private static Vector2 offset = new Vector2( 0, 3 );
    private static StringBuilder sb = new StringBuilder();

    static CustomHierarchyView()
    {
      EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    private static void HandleHierarchyWindowItemOnGUI( int instanceID, Rect selectionRect )
    {
      var obj = EditorUtility.InstanceIDToObject( instanceID );

      if( obj != null )
      {
        GameObject go = EditorUtility.InstanceIDToObject( instanceID ) as GameObject;


        //if( obj.name.Contains( "_" ) )

        //if( CustomHierarchyData.Contains( obj.name ) )
        {
          //string comment = "";
          Rect normalRect = new Rect( selectionRect.position, selectionRect.size );
          Rect offsetRect = new Rect( selectionRect.position + offset, selectionRect.size );


          //if( Selection.instanceIDs.( instanceID ) )
          //{
          //  fontColor = Color.white;
          //backgroundColor = Color.cyan;
          //  EditorGUI.DrawRect( selectionRect, backgroundColor );
          //}


          //else
          //{
          //  CustomHElement element = CustomHierarchyData.GetElement( obj.name );
          //  fontColor = element.fontColor;
          //  backgroundColor = element.color;
          //  comment = element.comment;

          //  Texture2D t = (Texture2D)AssetDatabase.LoadAssetAtPath( "Assets/CustomEditor/img/back.png", typeof( Texture2D ) );
          //  Texture2D text = new Texture2D( t.width, t.height );
          //  for( int x = 0; x < t.width; x++ )
          //  {
          //    for( int y = 0; y < t.height; y++ )
          //    {
          //      Color curColor = backgroundColor;
          //      curColor.a = t.GetPixel( x, y ).a;

          //      text.SetPixel( x, y, curColor );
          //    }
          //  }
          //  text.wrapMode = TextureWrapMode.Clamp;
          //  text.Apply();
          //  GUI.DrawTexture( selectionRect, text );
          //}


          //EditorGUI.LabelField( offsetRect, obj.name + " " + comment, new GUIStyle()
          //{
          //  normal = new GUIStyleState() { textColor = fontColor },
          //  fontStyle = FontStyle.Bold
          //} );

          if( go.GetComponent<BaseDirector>() != null )
          {
            EditorGUI.LabelField( normalRect, "Director", new GUIStyle()
            {
              normal = new GUIStyleState() { textColor = Color.green },
              fontStyle = FontStyle.Normal,
              alignment = TextAnchor.MiddleRight
            } );
          }

          if( go.GetComponent<Renderer>() != null )
          {
            EditorGUI.LabelField( normalRect, "Renderer", new GUIStyle()
            {
              normal = new GUIStyleState() { textColor = Color.green },
              fontStyle = FontStyle.Normal,
              alignment = TextAnchor.MiddleCenter
            } );
          }

          if( go.GetComponent<LODGroup>() != null )
          {
            EditorGUI.LabelField( normalRect, "LOD", new GUIStyle()
            {
              normal = new GUIStyleState() { textColor = Color.red },
              fontStyle = FontStyle.Normal,
              alignment = TextAnchor.MiddleCenter
            } );
          }

          if( go.layer != 0 )
          {

            string layerName = LayerMask.LayerToName( go.layer );
            if( layerName.Contains( "SmallObj" ) || layerName.Contains( "BigObj" ) )
            {
              EditorGUI.LabelField( normalRect, LayerMask.LayerToName( go.layer ), new GUIStyle()
              {
                normal = new GUIStyleState() { textColor = Color.yellow },
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.MiddleRight
              } );
            }
            else if
               ( layerName.Contains( "CamColli" ) )
            {
              EditorGUI.LabelField( normalRect, LayerMask.LayerToName( go.layer ), new GUIStyle()
              {
                normal = new GUIStyleState() { textColor = Color.cyan },
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.MiddleRight
              } );
            }
            else if( layerName.Contains( "Interac" ) )
            {
              EditorGUI.LabelField( normalRect, LayerMask.LayerToName( go.layer ), new GUIStyle()
              {
                normal = new GUIStyleState() { textColor = Color.green },
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.MiddleRight
              } );
            }
            else
            {
              EditorGUI.LabelField( normalRect, LayerMask.LayerToName( go.layer ), new GUIStyle()
              {
                normal = new GUIStyleState() { textColor = Color.gray },
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.MiddleRight
              } );
            }
          }

        }
      }
    }

  }

  struct ActivePassive
  {
    public int active;
    public int passive;
  }
}