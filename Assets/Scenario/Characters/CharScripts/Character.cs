using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//demographics are physical, undeniable descriptions of a person that can be discovered simply by seeing / meeting them.
public class Character
{
    //Demographics
    int id; //Secret id that keeps track of which character this is in the scenario (0-100). 0 is the murder victim.
    int height;
    int weight;
    bool isMale;
    public BodyType bodyType;
    public HeadType headType;
    public SkinTone skinTone;
    public HairStyle hairStyle;
    public HairColor hairColor;
    public FaceType face;
    CPD_Moustache moustache;
    CPD_Beard beard;
    CPD_Glasses glasses;
    CPD_EyeColor eyeColor;

    //Role - dependent on the template chosen.
    public Role role;

    //Attributes
    string firstName;
    string lastName;
    (int day, int month) birthday;
    int age;

    //Relationships
    int[] family; //Murderer fairly unlikely to kill family
    int[] friends; //Murderer VERY unlikely to kill a friend
    int[] contacts; //Can be a good or bad relationship
    int[] enemies; //Murderer fairly likely to kill enemies

    //Traits
    HashSet<string> traits;

    //Randomize everything!
    public Character(int id)
    {
        this.id = id;
    }

    public void randomizeDemographics()
    {
        height = CharRandomValue.range(0, 6);
        weight = CharRandomValue.range(0, 6);
        isMale = CharRandomValue.coin();
        headType = CPD_HeadType.getRandom();
        bodyType = CPD_BodyType.getRandom();
        face = CPD_Face.getRandom();
        skinTone = CPD_SkinTone.getRandom();
        hairColor = CPD_HairColor.getRandom();
        hairStyle = CPD_Hair.getRandom();

        //name
        (string f, string l) fullName = CharRandomValue.randomName(isMale);
        firstName = fullName.f;
        lastName = fullName.l;

        //traits
        traits = CharacterTrait.getRandomTraits(5);
    }

    public string getDisplayName(bool newline)
    {
        if (newline == false) return firstName + " " + lastName;
        else return firstName + "\n" + lastName;
    }

    public bool hasTrait(string name)
    {
        return traits.Contains(name);
    }

    public override string ToString()
    {
        string str = "[" + id + "] " + firstName + " " + lastName + "\n" +
            "Body: " + bodyType + "\n" +
            "Head: " + headType + "\n" +
            "Ht: " + height + "\n" +
            "Wt: " + weight + "\n" +
            "Male: " + isMale + "\n" +
            "SkinTone: " + skinTone + "\n" +
            "Hair: " + hairStyle + "," + hairColor + "\n" +
            "Role: " + role + "\n" +
            "NumTraits: " + traits.Count + "\n";
        return str;
    }
}
